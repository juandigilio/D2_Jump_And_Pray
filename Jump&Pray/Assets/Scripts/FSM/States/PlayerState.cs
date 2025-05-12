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

    public void EnablePlayerUpdate()
    {
        stateManager.PlayerController().enabled = true;
    }

    public void DisablePlayerUpdate()
    {
        stateManager.PlayerController().enabled = false;
    }

    public void LockCamera()
    {
        stateManager.Cameraman().enabled = false;
    }

    public void UnlockCamera()
    {
        stateManager.Cameraman().enabled = true;
    }
}
