using UnityEngine;

public class CinematicFall : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float controlStrength = 3f;
    [SerializeField] private float maxHorizontalSpeed = 1f;
    [SerializeField] private string landedSoundID = "Land";
    [SerializeField] private string wtfSoundID = "WhatTheFuck";

    private Rigidbody rb;
    private bool isCinematicFalling = false;
    private bool firstTime;

    private void Awake()
    {
        rb = GameManager.Instance.GetPlayerController().GetRigidbody();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the CinematicFall GameObject.");
        }
    }

    private void Start()
    {
        firstTime = true;
        isCinematicFalling = false;
    }

    private void FixedUpdate()
    {
        if (isCinematicFalling)
        {
            Vector3 horizontalTargetPos = new Vector3(target.position.x, rb.position.y, target.position.z);
            Vector3 toTarget = (horizontalTargetPos - rb.position);

            Vector3 horizontalDir = new Vector3(toTarget.x, 0f, toTarget.z).normalized;

            Vector3 newVelocity = rb.linearVelocity;

            Vector3 targetVelocityXZ = horizontalDir * maxHorizontalSpeed;
            newVelocity.x = Mathf.Lerp(rb.linearVelocity.x, targetVelocityXZ.x, controlStrength * Time.fixedDeltaTime);
            newVelocity.z = Mathf.Lerp(rb.linearVelocity.z, targetVelocityXZ.z, controlStrength * Time.fixedDeltaTime);

            rb.linearVelocity = newVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCinematicFalling && firstTime)
        {
            if (other.CompareTag("Player"))
            {
                firstTime = false;
                StartCinematicFall();
                GameManager.Instance.GetPlayerController().TurnOnCollider();
            }
        }
    }

    public void StartCinematicFall()
    {
        isCinematicFalling = true;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        EventManager.Instance.TriggerCinematicFallStarted();
    }

    public void StopCinematicFall()
    {
        if (isCinematicFalling)
        {
            Debug.Log("Cinematic fall started.");
            isCinematicFalling = false;
            GameManager.Instance.GetAudioManager().PlayCharacterFx(landedSoundID);
            GameManager.Instance.GetAudioManager().PlayCharacterFx(wtfSoundID);
        }    
    }

    public bool IsCinematicFalling()
    {
        return isCinematicFalling;
    }
}
