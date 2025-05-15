using UnityEngine;

public abstract class PlayerState
{
    protected StateManager stateManager;
    protected Cameraman cameraman;

    public PlayerState(StateManager stateManager, Cameraman cameraman)
    {
        this.stateManager = stateManager;
        this.cameraman = cameraman;
    }

    public virtual void Enter() { }
    public virtual void Enter(Vector3 cameraPosition, Vector3 target) { }
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

    public void SetCinematicCamera(Vector3 cameraPosition, Vector3 target)
    {
        cameraman.SetCinematicCamera(cameraPosition, target);
    }
}
