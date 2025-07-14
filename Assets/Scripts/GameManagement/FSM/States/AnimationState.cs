using UnityEngine;

public class AnimationState : BaseState
{
    public AnimationState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        DisablePlayerUpdate();
    }

    public override void LateUpdate()
    {
        cameraman.UpdateCamera();
    }

    public override void Exit()
    {
        EnablePlayerUpdate();
    }
}
