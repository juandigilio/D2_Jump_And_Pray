using UnityEngine;
using static MenuManager;

public class TriggerBehaviour : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;


    public enum TriggerType { Play, Options, Exit }
    public TriggerType type;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (type)
        {
            case TriggerType.Play:
            {
                menuManager.LoadGame();
                break;
            }
            case TriggerType.Options:
            {
                menuManager.LoadOptions();
                break;
            }
            case TriggerType.Exit:
            {
                menuManager.QuitGame();
                break;
            }
        }
    }
}
