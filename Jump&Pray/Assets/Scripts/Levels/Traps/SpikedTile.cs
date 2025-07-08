using UnityEngine;

public class SpikedTile : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                EventManager.Instance.TriggerPlayerKicked();
                playerController.SubtractLife();
                lastDamageTime = Time.time;
                Debug.Log("Player Spiked Tile!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                EventManager.Instance.TriggerPlayerKicked();
                playerController.SubtractLife();
                lastDamageTime = Time.time;
                Debug.Log("Player Spiked Tile!");
            }
        }
    }
}
