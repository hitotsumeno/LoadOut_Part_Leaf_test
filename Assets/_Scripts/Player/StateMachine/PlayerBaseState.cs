using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateManager StateManager;
    protected InputReader Reader;

    protected PlayerBaseState(PlayerStateManager manager, InputReader reader)
    {
        this.StateManager = manager;
        this.Reader = reader;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();

}
