using UnityEngine;

public abstract class BaseState
{
    protected StateManager stateManager;
    protected Cameraman cameraman;

    public BaseState(StateManager stateManager, Cameraman cameraman)
    {
        this.stateManager = stateManager;
        this.cameraman = cameraman;
    }

    public virtual void Enter() { }
    public virtual void Enter(Vector3 cameraPosition, GameObject target) { }
    public virtual void Enter(Vector3 start, Vector3 end) { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Exit() { }

    public void EnablePlayerUpdate()
    {
        GameManager.Instance.GetPlayerInput().enabled = true;
    }

    public void DisablePlayerUpdate()
    {
        GameManager.Instance.GetPlayerInput().enabled = false;
    }

    public void SetCinematicCamera(Vector3 cameraPosition, GameObject target)
    {
        cameraman.SetCinematicCamera(cameraPosition, target);
    }

    public void SetPausedInput()
    {
        GameManager.Instance.GetInputManager().SetMenuActionMap();
    }

    public void SetInGameInput()
    {
        GameManager.Instance.GetInputManager().SetInGameActionMap();
    }
}
