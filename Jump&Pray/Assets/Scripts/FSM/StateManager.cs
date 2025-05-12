using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private Cameraman cameraman;
    [SerializeField] private PlayerController playerController;

    private PlayerState currentState;
    public PlayerState groundedState;
    public PlayerState cinematicState;
    public PlayerState animationState;
    public PlayerState corridorState;

    private void Awake()
    {
        groundedState = new GroundedState(this);
        cinematicState = new CinematicState(this);
        animationState = new AnimationState(this);
        corridorState = new CorridorState(this);
    }

    private void Start()
    {
        TransitionToState(groundedState);
    }

    private void Update()
    {
        currentState.Update();
    }

    public void TransitionToState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void EnablePlayerUpdate()
    {
        playerController.enabled = true;
    }

    public void DisablePlayerUpdate()
    {
        playerController.enabled = false;
    }

    //public void DisableInput()
    //{
    //    Debug.Log("Input Disabled");
    //}

    //public void EnableInput()
    //{
    //    Debug.Log("Input Enabled");
    //}

    public void LockCamera()
    {
        cameraman.enabled = false;
    }

    public void UnlockCamera()
    {
        cameraman.enabled = true;
    }

    public void LockCameraToCorridor()
    {
        cameraman.LockToCorridor();
    }
}
