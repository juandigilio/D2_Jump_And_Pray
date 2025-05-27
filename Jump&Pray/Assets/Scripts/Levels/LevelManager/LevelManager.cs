using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConection levelConection;
    [SerializeField] private Coin[] coinsPull;
    //[SerializeField] private Transform startPosition;

    private PlayerController playerController;

    private bool isPlatformActivated = false;

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();

        if (!playerController)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }

        //EventManager.Instance.TriggerMenuLoaded(startPosition.position);
    }

    private void FixedUpdate()
    {
        CheckCoins();
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
}
