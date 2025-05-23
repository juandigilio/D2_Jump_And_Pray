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
       
    }

    private void OnEnable()
    {
        EventManager.Instance.OnAnimationStarted += SetAnimationState;
        EventManager.Instance.OnAnimationFinished += SetGroundedState;

        EventManager.Instance.OnCinematicStarted += SetCinematicState;
        EventManager.Instance.OnCinematicFinished += SetGroundedState;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnAnimationStarted -= SetAnimationState;
        EventManager.Instance.OnAnimationFinished -= SetGroundedState;

        EventManager.Instance.OnCinematicStarted -= SetCinematicState;
        EventManager.Instance.OnCinematicFinished -= SetGroundedState;
    }

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        cameraman = GameManager.Instance.GetCameraman();
        GameManager.Instance.RegisterStateManager(this);

        groundedState = new GroundedState(this, cameraman);
        cinematicState = new CinematicState(this, cameraman);
        animationState = new AnimationState(this, cameraman);
        corridorState = new CorridorState(this, cameraman);

        SetGroundedState();
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

    private void TransitionToState(PlayerState newState, Vector3 cameraPosition, GameObject target)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(cameraPosition, target);
    }

    private void SetGroundedState()
    {
        TransitionToState(groundedState);
    }

    private void SetCinematicState(Vector3 cameraPosition, GameObject target)
    {
        TransitionToState(cinematicState, cameraPosition, target);
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
