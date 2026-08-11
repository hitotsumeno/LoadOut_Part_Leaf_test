using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    [SerializeField] private float rotationCoef = -.5f;
    [SerializeField] private float idleDelay = 3f;
    [SerializeField] private float rotationSpeed = 0.15f;

    private Coroutine _idleRoutine;
    
    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpPressedEvent += HandleJump;

        Debug.Log("Player enters IDLE state");

        _idleRoutine = StartCoroutine(IdleRoutine());

    }

    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpPressedEvent -= HandleJump;

        if (_idleRoutine != null)
        {
            StopCoroutine(_idleRoutine);
            _idleRoutine = null;
        }
    }
    public override void FixedUpdateState()
    {
        if (!StateManager._groundCheck._isGrounded)
        {
            StateManager.SwitchStateTo(StateManager.airState);
            return;
        }
    }

    public override void UpdateState()
    {
        
    }

    private IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(idleDelay);

        while(true)
        {
            RotateCircle(rotationSpeed);
            yield return null;
        }
    }

    #region Handle Events
        private void HandleMove(float dir)
    {
        if (dir != 0f)
        {
            StateManager.SwitchStateTo(StateManager.moveState);
        } 
        else 
            return;
    }
    private void HandleJump()
    {
        StateManager.airState.QueueJump();
        StateManager.SwitchStateTo(StateManager.airState);
    }
    #endregion

}
