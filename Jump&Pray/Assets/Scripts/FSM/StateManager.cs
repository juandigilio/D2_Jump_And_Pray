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

    private void OnEnable()
    {
        DoorBehaviour.OnCinematicStarted += LockCamera;
    }

    private void OnDisable()
    {
        DoorBehaviour.OnCinematicStarted -= LockCamera;
    }

    private void Update()
    {
        currentState.Update();
    }

    private void TransitionToState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public Cameraman Cameraman()
    {
        return cameraman;
    }

    public PlayerController PlayerController()
    {
        return playerController;
    }
}
