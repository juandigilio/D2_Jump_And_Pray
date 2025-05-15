using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        cameraman.SetThirdPersonCamera();
        EnablePlayerUpdate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Grounded State");
    }
}
