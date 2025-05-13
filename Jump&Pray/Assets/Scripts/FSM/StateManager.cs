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
        DoorBehaviour.OnCinematicStarted += SetCinematicState;
        DoorBehaviour.OnCinematicEnded += SetGroundedState;
    }

    private void OnDisable()
    {
        DoorBehaviour.OnCinematicStarted -= SetCinematicState;
        DoorBehaviour.OnCinematicEnded -= SetGroundedState;
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

    private void SetGroundedState()
    {
        TransitionToState(groundedState);
    }

    private void SetCinematicState()
    {
        TransitionToState(cinematicState);
    }

    private void SetAnimationState()
    {
        TransitionToState(animationState);
    }

    private void SetCorridorState()
    {
        TransitionToState(corridorState);
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
