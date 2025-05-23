using UnityEngine;


public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float maxChargeTime = 1.0f;
    [SerializeField] private float minChargeTime = 0.05f;

    private Rigidbody rigidBody;

    private bool isJumping = false;
    private bool doubleJump = false;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float chargingStartTime;
    private float chargeTime;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found");
        }
    }

    private void Update()
    {
        CheckCharginTime();

        CheckGround();
    }

    private void CheckCharginTime()
    {
        if (isCharging)
        {
            if (Time.time - chargingStartTime >= maxChargeTime)
            {
                StopCharge();
            }
        }
    }

    public void StartCharge()
    {
        chargingStartTime = Time.time;

        isCharging = true;
    }

    public void StopCharge()
    {
        if (isCharging)
        {
            chargeTime = Time.time - chargingStartTime;

            if (chargeTime > maxChargeTime)
            {
                chargeTime = maxChargeTime;
            }
            else if (chargeTime < minChargeTime)
            {
                chargeTime = minChargeTime;
            }

            isCharging = false;

            Jump();
        }
    }

    private void Jump()
    {
        if (!isJumping && isGrounded)
        {
            Vector3 boostedForce = Vector3.up * (jumpForce * chargeTime);

            rigidBody.AddForce(boostedForce, ForceMode.Impulse);

            isJumping = true;
        }        
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
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }
    }
}
