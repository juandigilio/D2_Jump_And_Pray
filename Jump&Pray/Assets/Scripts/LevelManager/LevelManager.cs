using System.Xml.Schema;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private DoorBehaviour door;

    private bool isDoorOpened = false;
    private int totalCoins = 4;
    private int collectedCoins = 0;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        CheckCoins();
    }

    private void CheckCoins()
    {
        if (collectedCoins >= totalCoins && !isDoorOpened)
        {
            isDoorOpened = true;

            door.OpenDoor();
        }
    }
}
