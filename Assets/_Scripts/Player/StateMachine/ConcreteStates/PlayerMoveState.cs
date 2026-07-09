using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private Vector2 _movementDir;
    private Vector2 _moveVelocity;
    [SerializeField] private float groundAcceleration = 5f;
    [SerializeField] private float groundDeceleration = 20f;
    [SerializeField] private float horizontalMaxSpeed = 12.5f;
    [SerializeField] private float rotationCoef = -.5f;

    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpEvent += HandleJump;
        Debug.Log("Player enters Move state");
    }

 
    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpEvent -= HandleJump;
    }

    public override void UpdateState()
    {
        // throw new System.NotImplementedException();
    }
     public override void FixedUpdateState()
    {
        if (Move(groundAcceleration, groundDeceleration, _movementDir) <= 0.05f)
        {
            StateManager.SwitchStateTo(StateManager.idleState);
        }
    }

    private float Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0) * horizontalMaxSpeed;

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.deltaTime);
            StateManager._rb.velocity = new Vector2(_moveVelocity.x, StateManager._rb.velocity.y);
            RotateCircle(_moveVelocity.x);
            return Mathf.Abs(_moveVelocity.x);
        }

        if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity,Vector2.zero, deceleration * Time.deltaTime);
            StateManager._rb.velocity = new Vector2(_moveVelocity.x, StateManager._rb.velocity.y);
            RotateCircle(_moveVelocity.x);
            return Mathf.Abs(_moveVelocity.x);
        }
        
        return 0f;
    }

    private void RotateCircle(float cVelocity)
    {
        StateManager._spriteGO.transform.Rotate(0, 0, cVelocity * rotationCoef);
    }

    #region Handle Events
    private void HandleMove(float dir)
    {
        _movementDir.x = dir;
    }
    private void HandleJump()
    {
        StateManager.SwitchStateTo(StateManager.jumpState);
    }
#endregion
}
