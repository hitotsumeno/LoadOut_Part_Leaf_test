using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager manager, InputReader reader) : base(manager, reader) {}

    // Reader reference

    // handle var

    // event func

    public override void EnterState()
    {
        Reader.MoveEvent += HandleMove;
        Reader.JumpEvent += HandleJump;
    }

    public override void ExitState()
    {
        Reader.MoveEvent -= HandleMove;
        Reader.JumpEvent -= HandleJump;
    }
    public override void FixedUpdateState()
    {
        // throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        // throw new System.NotImplementedException();
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
    private void HandleJump()
    {
        StateManager.SwitchStateTo(StateManager.jumpState);
    }
}
