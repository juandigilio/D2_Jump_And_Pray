using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private Vector2 inputDirection;
    private bool isGrounded;
    private int availableLifes;
    private bool isTutorial;


    private void OnEnable()
    {
        EventManager.Instance.OnMenuLoaded += ResetPosition;
        EventManager.Instance.OnResetPlayer += TurnOffCollider;
    }

    private void OnDisable()
    {
        inputDirection = Vector3.zero;
        movementBehaviour.SetInputDirection(inputDirection);

        EventManager.Instance.OnMenuLoaded -= ResetPosition;
        EventManager.Instance.OnResetPlayer -= TurnOffCollider;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider == null)
        {
            Debug.LogError("CapsuleCollider not found on the GameObject.");
        }

        GameManager.Instance.RegisterPlayer(this);

        availableLifes = 1;
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

    private void TurnOffCollider()
    {
        capsuleCollider.enabled = false;
    }

    private void TurnOnCollider()
    {
        capsuleCollider.enabled = true;
    }

    private void ResetPosition(Vector3 startPos)
    {
        rigidBody.position = startPos;
    }

    public bool ResetPlayer(Vector3 startPos)
    {
        TurnOnCollider();

        if (SubtractLife())
        {
            ResetPosition(startPos);
            return true;
        }
        else
        {
            return false;
        }       
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

    public bool SubtractLife()
    {
        if (!isTutorial)
        {
            availableLifes--;
        } 

        if (availableLifes <= 0)
        {
            availableLifes = 3;
            EventManager.Instance.TriggerPlayerLost();

            return false;
        }
        else
        {
            return true;
        }  
    }

    public void AddLife()
    {
        availableLifes++;
    }

    public void AnimationFinished()
    {
        EventManager.Instance.TriggerAnimationFinished();
    }

    public void SetTutorial(bool isTutorial)
    {
        this.isTutorial = isTutorial;
    }

    public Vector3 GetVelocity()
    {
        return rigidBody.linearVelocity;
    }

    public Rigidbody GetRigidbody()
    {
        return rigidBody;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}