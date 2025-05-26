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


    private void OnDisable()
    {
        inputDirection = Vector3.zero;
        movementBehaviour.SetInputDirection(inputDirection);
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }

        movementBehaviour = GetComponent<MovementBehaviour>();
        if (movementBehaviour == null)
        {
            Debug.LogError("MovementBehaviour not found on the GameObject.");
        }

        jumpBehaviour = GetComponent<JumpBehaviour>();
        if (jumpBehaviour == null)
        {
            Debug.LogError("JumpBehaviour not found on the GameObject.");
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

        bool hit = Physics.Raycast(origin, Vector3.down, distance);

        Debug.DrawRay(origin, Vector3.down * distance, hit ? Color.green : Color.red);

        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        jumpBehaviour.SetGroundedCondition(isGrounded);
        movementBehaviour.SetGroundedCondition(isGrounded);
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

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public Vector3 GetVelocity()
    {
        return rigidBody.linearVelocity;
    }
}
