using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public bool isComplete { get; protected set; }

    protected float startTime;

    public float time => Time.time - startTime; // Properties with lambda exp, cannot be set
    protected PlayerStateManager StateManager;
    protected InputReader Reader;

     public virtual void Init(PlayerStateManager manager, InputReader reader)
    {
        StateManager = manager;
        Reader = reader;
    }

    public virtual void EnterState() {}
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState() {}
    public virtual void ExitState() {}

    protected float MoveHorizontal(float currentVelocity, float acceleration, float deceleration, float moveInput)
    {
        if (Mathf.Abs(moveInput) >= StateManager.MoveStats.MoveThreshold)
        {
            float targetVelocity = 0f;
            targetVelocity = moveInput * StateManager.MoveStats.horizontalMaxSpeed;

            currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            RotateCircle(currentVelocity);
        }

        if (Mathf.Abs(moveInput) <= StateManager.MoveStats.MoveThreshold)
        {
            currentVelocity = Mathf.Lerp(currentVelocity, 0f, deceleration * Time.fixedDeltaTime);
            RotateCircle(currentVelocity);
        }
        
        return currentVelocity;
    }

    protected void RotateCircle(float cVelocity)
    {
        StateManager._spriteGO.transform.Rotate(0, 0, cVelocity *  StateManager.MoveStats.rotationCoef);
    }
}
