using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 1f;
    [SerializeField] private float maxSpeed = 5f;

    private Vector2 movementInput;
    private Vector3 movementForce;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
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

    private void CalculateMovementForce()
    {
        //print(movementForce);
        movementForce = new Vector3(movementInput.x, 0, movementInput.y) * forceMultiplier;
    }

    private void AddForce()
    {
        CalculateMovementForce();
        rb.AddForce(movementForce, ForceMode.VelocityChange);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        //print(movementForce);
    }
}
