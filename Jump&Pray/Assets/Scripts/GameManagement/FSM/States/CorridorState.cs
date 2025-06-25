using UnityEngine;

public class CorridorState : BaseState
{
    public CorridorState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter(Vector3 cameraPosition, GameObject target)
    {
        Debug.Log("Entered Corridor State");
        cameraman.SetCorridorCamera(target, cameraPosition);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Corridor State");
        //UnlockCamera();
    }
}
