using UnityEngine;

public class CinematicState : PlayerState
{
    public CinematicState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Cinematic State");
        LockCamera();
        DisablePlayerUpdate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Cinematic State");
        UnlockCamera();
        EnablePlayerUpdate();
    }
}
