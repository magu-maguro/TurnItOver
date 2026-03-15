using UnityEngine;

public class CpuController : MonoBehaviour
{
    PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        InvokeRepeating(nameof(RandomMove), 0, 2);
    }

    void RandomMove()
    {
        Vector2 dir = Random.insideUnitCircle;

        movement.SetMoveInput(dir);

        if (Random.value < 0.3f)
            movement.TryJumpDrop();
    }
}