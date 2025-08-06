using Dreamteck.Splines;
using UnityEngine;

public class SplineCart : MonoBehaviour
{
    [SerializeField] private SplineFollower follower;
    [SerializeField] private float targetSpeed = 5f;
    [SerializeField] private float acceleration = 2f;

    private Vector3 playerOffset;
    private Vector3 startPos;
    private GameObject attachedPlayer;
    private bool isActivated = false;
    private float currentSpeed = 0f;

    private Rigidbody attachedRigidbody;
    private PlayerController attachedPlayerController;

    private void Start()
    {
        startPos = transform.position;

        ResetCart();
    }

    private void Update()
    {
        MoveCart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            EventManager.Instance.TriggerPlayerStartedDriving();

            StartRide(other.gameObject);
        }
    }

    private void StartRide(GameObject player)
    {
        isActivated = true;
        currentSpeed = 0f;

        AttachPlayer(player);
    }

    private void AttachPlayer(GameObject player)
    {
        attachedPlayer = player;
        playerOffset = player.transform.position - transform.position;
    }

    private void MoveCart()
    {
        if (isActivated && follower != null)
        {
            if (!follower.follow)
                follower.follow = true;

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            follower.followSpeed = currentSpeed;

            attachedPlayer.transform.position = transform.position + playerOffset;
            //attachedPlayer.transform.position = Vector3.Lerp(attachedPlayer.transform.position, transform.position + playerOffset, Time.deltaTime * 10f);
        }
    }

    private void ResetCart()
    {
        follower.follow = false;
        follower.motion.offset = Vector2.zero;
        follower.motion.rotationOffset = Vector3.zero;
        follower.motion.applyPositionX = true;
        follower.motion.applyPositionY = true;
        follower.motion.applyPositionZ = true;
        follower.motion.applyRotation = true;
        startPos = transform.position;
    }
}
