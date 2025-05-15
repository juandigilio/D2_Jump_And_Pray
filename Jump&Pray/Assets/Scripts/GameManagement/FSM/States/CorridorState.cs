using UnityEngine;

public class CorridorState : PlayerState
{
    public CorridorState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter(Vector3 cameraPosition, Vector3 target)
    {
        Debug.Log("Entered Corridor State");
        cameraman.SetCorridorCamera(cameraPosition, target);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Corridor State");
        //UnlockCamera();
    }
}
