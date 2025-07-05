using UnityEngine;

public class CinematicState : BaseState
{
    public CinematicState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }


    public override void Enter(Vector3 cameraPosition, GameObject target)
    {
        SetCinematicCamera(cameraPosition, target);
        DisablePlayerUpdate();
    }

    public override void Update()
    {
        cameraman.UpdateCamera();
    }

    public override void Exit()
    {
        EnablePlayerUpdate();
    }
}
