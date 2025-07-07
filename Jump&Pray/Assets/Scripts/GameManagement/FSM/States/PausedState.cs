using UnityEngine;

public class PausedState : BaseState
{
    public PausedState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        SetPausedInput();
    }

    public override void Exit()
    {
        SetInGameInput();
        Time.timeScale = 1f;
    }
}
