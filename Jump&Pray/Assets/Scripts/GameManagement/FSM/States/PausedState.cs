using UnityEngine;

public class PausedState : BaseState
{
    public PausedState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter()
    {
        Debug.Log("Entered paused State");
        SetPausedInput();
        Time.timeScale = 0.001f;
    }

    public override void Exit()
    {
        SetInGameInput();
        Time.timeScale = 1.0f;
    }
}
