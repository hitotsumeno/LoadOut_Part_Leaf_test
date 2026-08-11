using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMoveState : PlayerBaseState
{
    private float _movementDir;

    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpPressedEvent += HandleJump;
        // StateManager._groundCheck.isGroundedEvent += HandleIsGrounded;
        _movementDir = Reader.MoveDirections;
        Debug.Log("Player enters Move state");
    }

 
    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpPressedEvent -= HandleJump;
    }

    public override void UpdateState()
    {
        // throw new System.NotImplementedException();
    }
     public override void FixedUpdateState()
    {
        if (!StateManager._groundCheck._isGrounded)
        {
            StateManager.SwitchStateTo(StateManager.airState);
            return;
        }

        StateManager.p_HorizontalVelocity = MoveHorizontal( 
            StateManager.p_HorizontalVelocity, 
            StateManager.MoveStats.GroundAcceleration, 
            StateManager.MoveStats.GroundDeceleration, 
            _movementDir
        );

        if (Mathf.Abs(StateManager.p_HorizontalVelocity) <= 0.05f)
        {
            StateManager.SwitchStateTo(StateManager.idleState);
            return;
        }

    }

    #region Handle Events
    private void HandleMove(float dir)
    {
        _movementDir = dir;
    }
    private void HandleJump()
    {
        StateManager.airState.QueueJump();
        StateManager.SwitchStateTo(StateManager.airState);
    }
#endregion
}
