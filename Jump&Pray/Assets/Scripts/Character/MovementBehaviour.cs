using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector2 movementInput;
    private Vector3 movementDirection;
    private Vector2 horizontalVelocity;
    private Vector3 forward;
    private Vector3 right;

    private Rigidbody rigidBody;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }
    }

    private void FixedUpdate()
    {
        CalculateMovementDirection();
        LookForward();
        AddForce();
    }

    public void SetInputDirection(Vector2 input)
    {
        movementInput = input;

        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
    }

    private void LookForward()
    {
        Vector3 displacement = movementInput.x * right + movementInput.y * forward;

        if (displacement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(displacement);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void AddForce()
    {
        rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);

        horizontalVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
        }

        rigidBody.linearVelocity = new Vector3(horizontalVelocity.x, rigidBody.linearVelocity.y, horizontalVelocity.y);
    }

    private void CalculateMovementDirection()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        movementDirection = (forward * movementInput.y) + (right * movementInput.x);
        movementDirection *= forceMultiplier;
    }
}
