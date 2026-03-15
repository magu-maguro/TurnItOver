using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
    public bool isFaceUp = true; // カードが表向きかどうか
    private bool canFlip = true;
    public float flipCooldown = 0.1f;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && other.transform.parent.GetComponent<PlayerMovement>().isDroppedMoment && canFlip)
        {
            Flip();
        }
    }

    public void Flip()
    {
        if (!canFlip) return;

        isFaceUp = !isFaceUp;
        // 裏返るアニメーション開始
        StartCoroutine(FlipRoutine());
    }

    private IEnumerator FlipRoutine()
    {
        canFlip = false;
        // ここで回転アニメーションや見た目の変更
        Debug.Log("Card flipped! Now face " + (isFaceUp ? "up" : "down"));
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }
}
