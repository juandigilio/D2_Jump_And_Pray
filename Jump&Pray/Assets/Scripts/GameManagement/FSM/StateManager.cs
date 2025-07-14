using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Cameraman cameraman;

    private BaseState currentState;
    public BaseState groundedState;
    public BaseState cinematicState;
    public BaseState animationState;
    public BaseState corridorState;
    public BaseState pausedState;

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        EventManager.Instance.OnAnimationStarted += SetAnimationState;
        EventManager.Instance.OnAnimationFinished += SetGroundedState;

        EventManager.Instance.OnCinematicStarted += SetCinematicState;
        EventManager.Instance.OnCinematicFinished += SetGroundedState;

        EventManager.Instance.OnShowOptionsMenu += SetPausedState;
        EventManager.Instance.OnHideOptionsMenu += SetGroundedState;

        EventManager.Instance.OnMenuLoaded += SetGroundedState;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnAnimationStarted -= SetAnimationState;
        EventManager.Instance.OnAnimationFinished -= SetGroundedState;

        EventManager.Instance.OnCinematicStarted -= SetCinematicState;
        EventManager.Instance.OnCinematicFinished -= SetGroundedState;

        EventManager.Instance.OnShowOptionsMenu -= SetPausedState;
        EventManager.Instance.OnHideOptionsMenu -= SetGroundedState;

        EventManager.Instance.OnMenuLoaded -= SetGroundedState;
    }

    private void Start()
    {
        cameraman = GameManager.Instance.GetCameraman();
        GameManager.Instance.RegisterStateManager(this);

        groundedState = new GroundedState(this, cameraman);
        cinematicState = new CinematicState(this, cameraman);
        animationState = new AnimationState(this, cameraman);
        corridorState = new CorridorState(this, cameraman);
        pausedState = new PausedState(this, cameraman);

        SetGroundedState();
    }    

    private void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    private void LateUpdate()
    {
        currentState.LateUpdate();
    }

    private void TransitionToState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    private void TransitionToState(BaseState newState, Vector3 cameraPosition, GameObject target)
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

    private void SetGroundedState(Vector3 v)
    {
        SetGroundedState();
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

    private void SetPausedState()
    {
        TransitionToState(pausedState);
    }

    public Cameraman Cameraman()
    {
        return cameraman;
    }
}
