using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;

    PlayerMovement movement;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        movement = GetComponent<PlayerMovement>();

        inputActions.InGame.Move.performed += OnMove;
        inputActions.InGame.Move.canceled += OnMove;

        inputActions.InGame.JumpDrop.performed += OnJumpDrop;
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.InGame.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        movement.SetMoveInput(input);
    }

    void OnJumpDrop(InputAction.CallbackContext context)
    {
        movement.TryJumpDrop();
    }
}