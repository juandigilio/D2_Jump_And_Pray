using UnityEngine;

public abstract class PlayerState
{
    protected StateManager stateManager;

    public PlayerState(StateManager stateManager)
    {
        this.stateManager = stateManager;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
