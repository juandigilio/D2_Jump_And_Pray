using UnityEngine;


public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxChargeTime = 1.0f;
    [SerializeField] private float minChargeTime = 0.05f;
    [SerializeField] private string soundID = "Jump";

    private Rigidbody rigidBody;

    private bool isJumping = false;
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
            isJumping = true;

            AddForces();

            EventManager.Instance.TriggerPlayerJump();
            GameManager.Instance.GetAudioManager().PlayCharacterFx(soundID);
        }
    }

    private void AddForces()
    {
        Vector3 boostedForce = Vector3.up * (jumpForce * chargeTime);

        rigidBody.AddForce(boostedForce, ForceMode.Impulse);
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
}
