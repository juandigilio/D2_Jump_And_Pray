using UnityEngine;

public class CinematicState : PlayerState
{
    public CinematicState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }


    public override void Enter(Vector3 cameraPosition, Vector3 target)
    {
        SetCinematicCamera(cameraPosition, target);
        DisablePlayerUpdate();
    }

    public override void Exit()
    {
        EnablePlayerUpdate();
    }
}
