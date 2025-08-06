using UnityEngine;

public class GroundedState : BaseState
{
    public GroundedState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        cameraman.SetThirdPersonCamera();
        SetInGameInput();
        EnablePlayerUpdate();
    }

    public override void LateUpdate()
    {
        cameraman.UpdateCamera();
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Grounded State");
    }
}