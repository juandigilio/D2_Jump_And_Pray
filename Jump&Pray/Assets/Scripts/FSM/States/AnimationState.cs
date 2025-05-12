using UnityEngine;

public class AnimationState : PlayerState
{
    public AnimationState(StateManager stateManager) : base(stateManager) { }

    public override void Enter()
    {
        Debug.Log("Entered Animation State");
        stateManager.DisablePlayerUpdate();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Animation State");
        stateManager.EnablePlayerUpdate();
    }
}
