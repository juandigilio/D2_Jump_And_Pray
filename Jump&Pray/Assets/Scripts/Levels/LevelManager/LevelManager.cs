using System.Xml.Schema;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DoorBehaviour door;
    [SerializeField] private Coin[] coinsPull;

    private PlayerController playerController;

    private bool isDoorOpened = false;

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
    }

    private void CheckCoins()
    {
        if (!isDoorOpened)
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
                isDoorOpened = true;
                door.Open();
            }
        }  
    }
}
