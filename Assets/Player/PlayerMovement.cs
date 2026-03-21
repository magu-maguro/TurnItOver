using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [Header("水平移動")]
    bool canMove = true;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 12f;
    Vector3 moveInput;

    [Header("ジャンプドロップ")]
    [SerializeField] Transform modelTransform; // 前転のためにModelだけ回転させる
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 0.3f; // 上昇時間
    [SerializeField] float hangTime = 0.2f;     // 空中で前転する時間
    [SerializeField] float pauseTime = 0.2f;   // 頂点での一瞬の停止時間
    [SerializeField] float dropDuration = 0.2f; // 急降下時間
    [SerializeField] float cooldown = 1.2f;
    bool isCooldown;

    [Header("エフェクト")]
    [SerializeField] ParticleSystem dropEffectPrefab;
    ParticleSystem dropEffectInstance;

    public bool isDroppedMoment;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dropEffectInstance = Instantiate(dropEffectPrefab, transform.position, Quaternion.identity);
        dropEffectInstance.Stop();
    }
    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    void Move()
    {
        if (!canMove) return;
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 velocity = dir * moveSpeed;

        // Y軸の速度はそのまま残す
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    void Rotate()
    {
        if (!canMove) return;

        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    public void TryJumpDrop()
    {
        if (isCooldown) return;

        StartCoroutine(JumpDropRoutine());
    }

    IEnumerator JumpDropRoutine()
    {
        isCooldown = true;

        float startY = rb.position.y;

        // jump up
        float t = 0;
        while (t < jumpDuration)
        {
            t += Time.fixedDeltaTime;
            float y = Mathf.Lerp(0, jumpHeight, t / jumpDuration);
            rb.MovePosition(new Vector3(rb.position.x, startY + y, rb.position.z));
            yield return new WaitForFixedUpdate();
        }

        // hang
        canMove = false;
        rb.linearVelocity = Vector3.zero; // 空中での移動を止める

        t = 0;
        Quaternion baseRotation = modelTransform.localRotation;
        while (t < hangTime)
        {
            t += Time.fixedDeltaTime;
            float angle = Mathf.Lerp(0, 360, t / hangTime); // 前転
            modelTransform.localRotation = baseRotation * Quaternion.Euler(angle, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        modelTransform.localRotation = baseRotation;

        // short pause at the top
        yield return new WaitForSeconds(pauseTime);

        // drop
        t = 0;
        while (t < dropDuration)
        {
            t += Time.fixedDeltaTime;
            float y = Mathf.Lerp(jumpHeight, 0, t / dropDuration);
            rb.MovePosition(new Vector3(rb.position.x, startY + y, rb.position.z));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(new Vector3(rb.position.x, startY, rb.position.z));

        canMove = true;
        
        isDroppedMoment = true;
        // effect
        dropEffectInstance.transform.position = transform.position;
        dropEffectInstance.Play();
        StartCoroutine(SoundManager.PlaySE(0));
        yield return new WaitForSeconds(0.05f);
        isDroppedMoment = false;

        // cooldown
        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
    }
}