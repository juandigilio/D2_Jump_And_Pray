using System;
//using System.Diagnostics;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 1f;
    [SerializeField] private float onAirForceMultiplier = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float gravityMultiplier = 10f;
    [SerializeField] private float rollingMaxSpeed = 8f;
    [SerializeField] private float rotationSpeed = 9f;
    [SerializeField] private float decelerationSpeed = 8f;

    private Vector2 movementInput;
    private Vector3 movementDirection;
    private Vector2 horizontalVelocity;
    private Vector3 forward;
    private Vector3 right;
    private bool isGrounded = true;
    private bool isRolling = false;

    private Rigidbody rigidBody;


    private void OnEnable()
    {
        EventManager.Instance.OnRollFinished += RollFinished;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnRollFinished -= RollFinished;
    }

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
        StopInertia();
        AddForces();
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

    private void AddForces()
    {
        rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);

        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.down * gravityMultiplier, ForceMode.VelocityChange);
        }

        NormalizeVelocity();
    }

    private void NormalizeVelocity()
    {
        horizontalVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.z);

        if (isRolling)
        {
            if (horizontalVelocity.magnitude > rollingMaxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * rollingMaxSpeed;
            }
        }
        else
        {
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            }
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
        //Vector3 normal = Vector3.up;
        //movementDirection = Vector3.ProjectOnPlane(movementDirection, normal);
        if (isGrounded)
        {
            movementDirection *= forceMultiplier;
        }
        else
        {
            movementDirection *= onAirForceMultiplier;
        }
    }

    private void StopInertia()
    {
        if (movementInput == Vector2.zero && !isRolling)
        {
            Vector3 velocity = rigidBody.linearVelocity;

            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

            if (horizontalVelocity.magnitude > 0.1f)
            {
                float deceleration = isGrounded ? decelerationSpeed : decelerationSpeed * 0.25f;

                Vector3 decelerationVector = -horizontalVelocity.normalized * deceleration * Time.fixedDeltaTime;

                if (decelerationVector.magnitude > horizontalVelocity.magnitude)
                {
                    horizontalVelocity = Vector3.zero;
                }
                else
                {
                    horizontalVelocity += decelerationVector;
                }

                rigidBody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
            }
        }
    }

    private void RollFinished()
    {
        isRolling = false;
    }

    public void Roll()
    {
        if (!isRolling && isGrounded)
        {
            EventManager.Instance.TriggerPlayerRolled();

            rigidBody.AddForce(movementDirection, ForceMode.Impulse);

            NormalizeVelocity();

            isRolling = true;
        }
    }

    public void SetInputDirection(Vector2 input)
    {
        movementInput = input;

        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
    }

    public void SetGroundedCondition(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    public void SetRollCondition(bool isRolling)
    {
        this.isRolling = isRolling;
    }
}
