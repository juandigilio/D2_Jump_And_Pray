using UnityEngine;


public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private float maxChargeTime = 1.0f;
    [SerializeField] private float minChargeTime = 0.05f;

    private Rigidbody rigidBody;

    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float chargingStartTime;
    private float chargeTime;
    private bool isRolling = false;


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

    private void Jump()
    {
        if (!isJumping && isGrounded)
        {
            EventManager.Instance.TriggerPlayerJump();

            Vector3 boostedForce = Vector3.up * (jumpForce * chargeTime);

            rigidBody.AddForce(boostedForce, ForceMode.Impulse);

            isJumping = true;
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

    public void SetGroundedCondition(bool isGrounded)
    {
        this.isGrounded = isGrounded;

        if (isGrounded)
        {
            isJumping = false;
        }
    }

    public void SetRollCondition(bool isRolling)
    {
        this.isRolling = isRolling;

        if (isRolling)
        {
            isJumping = false;
        }
    }
}
