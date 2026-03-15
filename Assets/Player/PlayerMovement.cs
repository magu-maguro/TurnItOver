using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 12f;

    [SerializeField] float jumpForce = 7f;
    [SerializeField] float cooldown = 1.2f;

    bool isCooldown;

    Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    void Move()
    {
        Vector3 velocity = moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    void Rotate()
    {
        if (moveDirection.sqrMagnitude < 0.001f)
            return;

        Quaternion target = Quaternion.LookRotation(moveDirection);

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

        // ジャンプ
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.4f);

        // ドロップ
        rb.AddForce(Vector3.down * jumpForce * 1.5f, ForceMode.Impulse);

        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
    }
}