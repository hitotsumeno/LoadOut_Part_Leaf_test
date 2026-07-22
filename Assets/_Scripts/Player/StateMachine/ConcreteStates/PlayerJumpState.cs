using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    [SerializeField] private float VerticalForce = 9f;

    // Jump Event var
    bool JumpWasPressed;
    bool JumpWasRelease;
    bool JumpIsHeld;

    // Jump var
    public float VerticalVelocity {get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private bool _fastFallTime;
    private bool _fastFallRealeaseSpeed;
    private int _numberOfJumpUsed;

    // Apex var
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexTreshold;

    // Jump buffer var
    private float _jumpBufferTimer;
    private bool _jumpReleaseDuringBuffer;

    // Coyote Time var

    private float _coyoteTimer;




    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpPressedEvent += HandleJump;
        // Reader.JumpIsHeldEvent +=
        // Reader.JumpReleaseEvent +=
        Jump();
        Debug.Log("Player enters Jump state");
    }
    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpPressedEvent -= HandleJump;
    }

    public override void FixedUpdateState()
    {
        // throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        
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
