using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConection levelConection;
    [SerializeField] private Coin[] coinsPull;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform deathPoint;
    [SerializeField] private CinematicFall cinematicFall;

    private PlayerController playerController;

    private bool isPlatformActivated = false;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();

        if (!playerController)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }
    }

    private void FixedUpdate()
    {
        CheckCoins();
        CheckPlayerStatus();
    }

    private void CheckCoins()
    {
        if (!isPlatformActivated)
        {
            bool allCoinsCollected = true;

            foreach (Coin coin in coinsPull)
            {
                if (coin.gameObject.activeSelf)
                {
                    allCoinsCollected = false;
                    break;
                }
            }
            if (allCoinsCollected)
            {
                isPlatformActivated = true;
                levelConection.ActivatePlatform();
            }
        }
    }

    private void CheckPlayerStatus()
    {
        if (playerController.GetRigidbody().position.y < deathPoint.position.y)
        {
            playerController.SubtractLife();
            playerController.ResetPosition(startPoint.position);
            cinematicFall.StartCinematicFall();
        }
    }
}