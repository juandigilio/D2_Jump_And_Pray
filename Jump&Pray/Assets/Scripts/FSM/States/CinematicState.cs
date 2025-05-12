using UnityEngine;

public class CinematicState : PlayerState
{
    public CinematicState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Cinematic State");
        stateManager.LockCamera();
        stateManager.DisablePlayerUpdate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Cinematic State");
        stateManager.UnlockCamera();
        stateManager.EnablePlayerUpdate();
    }
}
