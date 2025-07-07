using UnityEngine;

public class SawBlade : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float moveSpeed = 2f;


    private PlayerController playerController;
    private Vector3 targetPosition;
    private float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not found in the GameManager.");
        }

        if (pointB != null)
        {
            targetPosition = pointB.position;
        }         
    }

    private void Update()
    {
        Rotate();
        MoveTowards();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                playerController.SubtractLife();
                lastDamageTime = Time.time;
                Debug.Log("Player hit by saw blade!");
            }
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void MoveTowards()
    {
        if (pointA != null && pointB != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
            }
        }
    }
}
