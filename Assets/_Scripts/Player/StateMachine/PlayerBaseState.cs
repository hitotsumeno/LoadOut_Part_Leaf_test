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

}
