using TMPro;
using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;


    private PlayerController playerController;
    private float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not found in the GameManager.");
        }
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
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
}
