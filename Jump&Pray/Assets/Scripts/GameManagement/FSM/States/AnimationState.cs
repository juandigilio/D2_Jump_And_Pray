using UnityEngine;

public class AnimationState : BaseState
{
    public AnimationState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        Debug.Log("Entered Animation State");
        DisablePlayerUpdate();
    }

    public override void Exit()
    {
        EnablePlayerUpdate();
    }
}
