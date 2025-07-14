using UnityEngine;

public class CorridorState : BaseState
{
    public CorridorState(StateManager stateManager, Cameraman cameraman) : base(stateManager, cameraman) { }

    public override void Enter(Vector3 start, Vector3 end)
    {
        cameraman.SetCorridorCamera(start, end);
    }

    public override void Exit()
    {

    }
}
