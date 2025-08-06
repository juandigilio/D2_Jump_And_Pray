using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private bool godMode = false;  
    [SerializeField] private string landSoundID = "Land";
    [SerializeField] private string wtfSoundID = "WhatTheFuck";
    [SerializeField] private string hitSoundID = "Hit";

    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private RailSwitcher railSwitcher;
    private Vector2 inputDirection;
    private bool isGrounded;
    private int availableLifes;
    private bool isTutorial;
    private bool isFirstTime = true;
    private bool isFalling = false;


    private void OnEnable()
    {
        EventManager.Instance.OnMenuLoaded += ResetPosition;
        EventManager.Instance.OnResetPlayer += TurnOffCollider;
        EventManager.Instance.OnPlayerSmashed += PlayHittedSound;
        EventManager.Instance.OnPlayerStartedDriving += LockDriving;
        EventManager.Instance.OnPlayerStoppedDriving += UnlockDriving;
    }

    private void OnDisable()
    {
        inputDirection = Vector3.zero;
        movementBehaviour.SetInputDirection(inputDirection);

        EventManager.Instance.OnMenuLoaded -= ResetPosition;
        EventManager.Instance.OnResetPlayer -= TurnOffCollider;
        EventManager.Instance.OnPlayerSmashed -= PlayHittedSound;
        EventManager.Instance.OnPlayerStartedDriving -= LockDriving;
        EventManager.Instance.OnPlayerStoppedDriving -= UnlockDriving;
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

        availableLifes = 3;
        isFirstTime = true;
        isTutorial = false;
    }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float totalDistance = groundCheckDistance + 0.1f;

        RaycastHit hit;

        bool isHit = Physics.SphereCast(
            origin,
            groundCheckRadius,
            Vector3.down,
            out hit,
            totalDistance,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        Debug.DrawRay(origin, Vector3.down * totalDistance, isHit ? Color.green : Color.red);
        Debug.DrawLine(origin + Vector3.right * groundCheckRadius, origin + Vector3.down * totalDistance + Vector3.right * groundCheckRadius, Color.blue);
        Debug.DrawLine(origin + Vector3.left * groundCheckRadius, origin + Vector3.down * totalDistance + Vector3.left * groundCheckRadius, Color.blue);

        UpdateGroundedCondition(isHit);
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

    public void TurnOnCollider()
    {
        capsuleCollider.enabled = true;
    }

    private void ResetPosition(Vector3 startPos)
    {
        rigidBody.position = startPos;
    }

    private void PlayHittedSound()
    {
        GameManager.Instance.GetAudioManager().PlayCharacterFx(hitSoundID);
    }

    public void GodMode()
    {
        godMode = !godMode;
    }

    public bool ResetPlayer(Vector3 startPos)
    {
        TurnOnCollider();

        if (SubtractLife(true))
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

    public void SwitchLeft()
    {
        if (railSwitcher != null)
        {
            railSwitcher.SwitchLeft();
        }
    }

    public void SwitchRight()
    {
        if (railSwitcher != null)
        {
            railSwitcher.SwitchRight();
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool SubtractLife(bool isFalling)
    {
        if (!godMode)
        {
            if (!isTutorial)
            {
                availableLifes--;
            }

            if (!isFalling)
            {
                EventManager.Instance.TriggerPlayerKicked();
            }           
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

    public bool SubtractLife()
    {
        return SubtractLife(false);
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

    public bool IsFirstTime()
    {
        return isFirstTime;
    }

    public void SetFirstTime(bool firstTime)
    {
        isFirstTime = firstTime;
    }

    private void LockDriving()
    {
        movementBehaviour.enabled = false;
        jumpBehaviour.enabled = false;
        rigidBody.isKinematic = true;
    }

    private void UnlockDriving()
    {
        movementBehaviour.enabled = true;
        jumpBehaviour.enabled = true;
        rigidBody.isKinematic = false;
    }

    public void SetRailSwitcher(RailSwitcher railSwitcher)
    {
        this.railSwitcher = railSwitcher;
    }
}