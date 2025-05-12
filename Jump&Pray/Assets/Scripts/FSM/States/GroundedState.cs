using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Grounded State");
        stateManager.EnablePlayerUpdate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Grounded State");
    }
}
