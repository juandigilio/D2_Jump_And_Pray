using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConection levelConection;
    [SerializeField] private Coin[] coinsPull;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform deathPoint;
    [SerializeField] private CinematicFall cinematicFall;
    [SerializeField] private RollingCrusher rollingCrusher;
    [SerializeField] private bool isTutorial = false;

    private PlayerController playerController;

    private bool isPlatformActivated = false;
    private bool isGameOver = false;
    private bool isCurrentLevel = false;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        playerController.SetTutorial(isTutorial);

        if (!playerController)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }

        isGameOver = false;
        isCurrentLevel = true;

        EventManager.Instance.OnPlayerLost += GoToGameOver;
        EventManager.Instance.OnResetGame += ResetGame;
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
        if (!isGameOver)
        {
            if (playerController.GetRigidbody().position.y < deathPoint.position.y)
            {
                ResetPlayer();
            }
        }  
    }

    private void ResetPlayer()
    {
        if (!isGameOver)
        {
            if (playerController.ResetPlayer(startPoint.position))
            {
                if (rollingCrusher)
                {
                    rollingCrusher.Reset();
                }
                cinematicFall.StartCinematicFall();
            }
            else
            {
                isGameOver = true;
                GoToGameOver();
            }
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadMenuScene();
    }

    private void GoToGameOver()
    {
        SceneManager.LoadMenuScene();
    }

    public void TurnOffLevel()
    {
        isCurrentLevel = false;
    }
}