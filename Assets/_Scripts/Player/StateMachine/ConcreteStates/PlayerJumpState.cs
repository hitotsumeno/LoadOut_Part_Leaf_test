using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    [SerializeField] private float VerticalForce = 9f;

    public PlayerJumpState(PlayerStateManager manager, InputReader reader) : base(manager, reader){}

    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpEvent += HandleJump;
        Jump();
    }
    public override void ExitState()
    {
        Reader.MoveEvent -=HandleMove;
        Reader.JumpEvent -= HandleJump;
    }

    public override void FixedUpdateState()
    {
        // throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        // throw new System.NotImplementedException();
    }
    private void Jump()
    {
        if (StateManager._groundCheck._isGrounded)
        {
            StateManager._rb.velocity = new Vector2(StateManager._rb.velocity.x, VerticalForce);
        }
    }

    #region Handle Events
    private void HandleJump()
    {
        Jump();
    }
        private void HandleMove(float dir)
    {
            if (dir != 0f)
        {
            StateManager.SwitchStateTo(StateManager.moveState);
        } 
        else 
            return;
    }
#endregion
}
