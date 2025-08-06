using UnityEngine;

public class DrivingState : BaseState
{
    public DrivingState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        SetDrivingCamera();
        SetDrivingInput();
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
