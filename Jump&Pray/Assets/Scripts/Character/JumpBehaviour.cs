using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float maxJumpCharge = 1.0f;
    [SerializeField] private float minJumpCharge = 0.2f;

    private Rigidbody rb;

    private bool isJumping = false;
    private bool doubleJump = false;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float chargingStartTime;
    private float chargeTime;


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
        CheckCharginTime();
    }

    public void StartCharge()
    {
        if (!isJumping && isGrounded)
        {
            chargingStartTime = Time.time;

            isCharging = true;
        }
    }

    public void StopCharge()
    {
        if (isCharging)
        {
            chargeTime = Time.time - chargingStartTime;

            if (chargeTime > maxJumpCharge)
            {
                chargeTime = maxJumpCharge;
            }
            else if (chargeTime < minJumpCharge)
            {
                chargeTime = minJumpCharge;
            }

            isCharging = false;

            Jump();
        }  
    }

    private void CheckCharginTime()
    {
        if (isCharging)
        {
            if (Time.time - chargingStartTime >= maxJumpCharge)
            {
                StopCharge();
            }
        }
    }

    private void Jump()
    {
        Vector3 boostedForce = Vector3.up * (jumpForce * chargeTime);

        rb.AddForce(boostedForce, ForceMode.Impulse);
    }
}
