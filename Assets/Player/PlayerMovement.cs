using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDuration = 0.4f;
    [SerializeField] float cooldown = 1.5f;

    bool isCooldown;

    Vector3 moveDirection;

    void Update()
    {
        Move();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    void Move()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void TryJumpDrop()
    {
        if (isCooldown) return;

        StartCoroutine(JumpDropRoutine());
    }

    IEnumerator JumpDropRoutine()
    {
        isCooldown = true;

        Vector3 start = transform.position;

        float timer = 0;

        // ジャンプ
        while (timer < jumpDuration)
        {
            timer += Time.deltaTime;

            float height = Mathf.Sin(timer / jumpDuration * Mathf.PI) * jumpHeight;

            transform.position = start + Vector3.up * height;

            yield return null;
        }

        // ドロップ
        transform.position = start;

        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
    }
}