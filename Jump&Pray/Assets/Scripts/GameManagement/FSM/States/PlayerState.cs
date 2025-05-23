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
    public virtual void Enter(Vector3 cameraPosition, GameObject target) { }
    public virtual void Update() { }
    public virtual void Exit() { }

    public void EnablePlayerUpdate()
    {
        //GameManager.Instance.GetPlayerController().enabled = true;
        //GameManager.Instance.GetInputManager().enabled = true;
        GameManager.Instance.GetPlayerInput().enabled = true;
    }

    public void DisablePlayerUpdate()
    {
        //GameManager.Instance.GetPlayerController().enabled = false;
        //GameManager.Instance.GetInputManager().enabled = false;
        GameManager.Instance.GetPlayerInput().enabled = false;
    }

    public void SetCinematicCamera(Vector3 cameraPosition, GameObject target)
    {
        cameraman.SetCinematicCamera(cameraPosition, target);
    }
}
