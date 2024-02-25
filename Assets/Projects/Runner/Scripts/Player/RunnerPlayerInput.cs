using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public interface IInput
{
    public event Action<Vector2> InputChanged;
}

public class RunnerPlayerInput : PoolableComponent, IInput
{
    public event Action<Vector2> InputChanged;

    [Header("Character Input Values")]
    public Vector2 move;
    public bool jump;
    private bool _isActivated;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
        InputChanged?.Invoke(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
        InputChanged?.Invoke(move);
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    
    public void Activate()
    {
        _isActivated = true;
    }
}
