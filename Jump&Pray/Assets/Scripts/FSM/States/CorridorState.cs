using UnityEngine;

public class CorridorState : PlayerState
{
    public CorridorState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Corridor State");
        LockCamera();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Corridor State");
        UnlockCamera();
    }
}
