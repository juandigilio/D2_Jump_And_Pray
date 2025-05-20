using UnityEngine;


public class TriggerBehaviour : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;


    public enum TriggerType { Play, Unload, Options, Exit }
    public TriggerType type;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding");

        switch (type)
        {
            case TriggerType.Play:
            {
                menuManager.LoadGame();
                break;
            }
            case TriggerType.Unload:
            {
                menuManager.Unload();
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
