using UnityEngine;

public class CpuController : MonoBehaviour
{
    PlayerMovement movement;

    private bool canMove = true;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        InvokeRepeating(nameof(RandomMove), 0, 2);
    }

    void RandomMove()
    {
        Vector2 dir = Random.insideUnitCircle;
        if (!canMove)
        {
            dir = Vector2.zero;
        }

        movement.SetMoveInput(dir);

        if (Random.value < 0.3f && canMove)
            movement.TryJumpDrop();
    }

    //-----------api
    public void DisallowInput()
    {
        canMove = false;
    }
    public void AllowInput()
    {
        canMove = true;
    }
}