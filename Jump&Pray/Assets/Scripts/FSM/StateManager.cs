using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Cameraman cameraman;
    private PlayerController playerController;

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

    private void OnEnable()
    {
        GameManager.Instance.RegisterStateManager(this);

        EventManager.Instance.OnCinematicStarted += SetCinematicState;
        EventManager.Instance.OnCinematicEnded += SetGroundedState;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnCinematicStarted -= SetCinematicState;
        EventManager.Instance.OnCinematicEnded -= SetGroundedState;
    }

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        cameraman = GameManager.Instance.GetCameraman();

        TransitionToState(groundedState);
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

    private void SetCinematicState(Vector3 cameraPosition, Vector3 target)
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
