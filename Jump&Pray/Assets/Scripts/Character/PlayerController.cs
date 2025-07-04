using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rigidBody;
    private Vector2 inputDirection;
    private bool isGrounded;


    private void OnEnable()
    {
        EventManager.Instance.OnMenuLoaded += MoveToMainMenu;
    }

    private void OnDisable()
    {
        inputDirection = Vector3.zero;
        movementBehaviour.SetInputDirection(inputDirection);

        EventManager.Instance.OnMenuLoaded -= MoveToMainMenu;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }

        GameManager.Instance.RegisterPlayer(this);
    }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position;
        float distance = groundCheckDistance;

        bool hit = Physics.Raycast(origin, Vector3.down, distance, ~0, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(origin, Vector3.down * distance, hit ? Color.green : Color.red);

        UpdateGroundedCondition(hit);
    }

    private void UpdateGroundedCondition(bool hit)
    {
        if (hit)
        {
            if (!isGrounded)
            {
                EventManager.Instance.TriggerPlayerLanded();
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

        jumpBehaviour.SetGroundedCondition(isGrounded);
        movementBehaviour.SetGroundedCondition(isGrounded);
    }

    private void MoveToMainMenu(Vector3 startPos)
    {
        rigidBody.position = startPos;
    }

    public void LoadJumpCharge()
    {
        jumpBehaviour.StartCharge();
    }

    public void ReleaseJumpCharge()
    {
        jumpBehaviour.StopCharge();
    }

    public void SetDirection(Vector2 input)
    {
        inputDirection = input;
        movementBehaviour.SetInputDirection(inputDirection);
    }

    public void Roll()
    {
        movementBehaviour.Roll();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void AnimationFinished()
    {
        EventManager.Instance.TriggerAnimationFinished();
    }

    public Vector3 GetVelocity()
    {
        return rigidBody.linearVelocity;
    }

    public Rigidbody GetRigidbody()
    {
        return rigidBody;
    }
}
