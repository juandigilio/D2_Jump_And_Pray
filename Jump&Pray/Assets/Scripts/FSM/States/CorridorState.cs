using UnityEngine;

public class CorridorState : PlayerState
{
    public CorridorState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Corridor State");
        stateManager.LockCameraToCorridor();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Corridor State");
        stateManager.UnlockCamera();
    }
}
