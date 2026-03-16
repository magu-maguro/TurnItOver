using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
    Rigidbody rb;
    public bool isFaceUp = true; // カードが表向きかどうか
    private bool canFlip = true;
    public float flipCooldown = 0.1f;

    public float flipForce = 2f;       // 浮かせる力
    public float flipDuration = 0.5f;  // 回転にかける時間

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerMovement>().isDroppedMoment && canFlip)
        {
            Vector3 vec = (transform.position - other.transform.position).normalized;
            Vector3 direction = new Vector3(vec.x, 0, vec.z).normalized;
            Flip(direction);
        }
    }

    public void Flip(Vector3 direction)
    {
        if (!canFlip) return;

        isFaceUp = !isFaceUp;
        StartCoroutine(FlipRoutine(direction));
    }

    private IEnumerator FlipRoutine(Vector3 direction)
    {
        canFlip = false;

        // 1. RigidBodyに少し浮かせる力を加える
        rb.AddForce((Vector3.up * flipForce + direction * flipForce * 0.5f), ForceMode.Impulse);

        // 2. 回転アニメーション
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;
        Vector3 flipAxis = direction.sqrMagnitude > 0.0001f
            ? Vector3.Cross(direction.normalized, Vector3.up)
            : transform.right;
        Quaternion endRot = Quaternion.AngleAxis(180f, flipAxis) * startRot; // direction側へ倒れ込む180°回転

        while (elapsed < flipDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / flipDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot; // 最終調整

        // 3. クールタイム
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;

        Debug.Log("Card flipped! Now face " + (isFaceUp ? "up" : "down"));
    }

    public void FloatCard()
    {
        rb.AddForce(Vector3.up * flipForce, ForceMode.Impulse);
        StartCoroutine(SoundManager.PlaySE(4));
    }
}