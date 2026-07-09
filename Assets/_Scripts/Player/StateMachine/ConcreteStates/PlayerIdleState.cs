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
        Reader.JumpEvent += HandleJump;

        Debug.Log("Player enters IDLE state");

        _idleRoutine = StartCoroutine(IdleRoutine());

    }

    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpEvent -= HandleJump;

        if (_idleRoutine != null)
        {
            StopCoroutine(_idleRoutine);
            _idleRoutine = null;
        }
    }
    public override void FixedUpdateState()
    {
        
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

    private void RotateCircle(float cVelocity)
    {
        StateManager._spriteGO.transform.Rotate(0, 0, cVelocity * rotationCoef);
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
        StateManager.SwitchStateTo(StateManager.jumpState);
    }
    #endregion

}
