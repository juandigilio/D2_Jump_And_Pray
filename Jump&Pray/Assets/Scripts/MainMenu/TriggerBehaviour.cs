using UnityEngine;


public class TriggerBehaviour : MonoBehaviour
{
    public enum TriggerType { Play, Unload, Options, Exit }
    public TriggerType type;

    [SerializeField] private MenuManager menuManager;


    private bool fitstTime;

    public void Awake()
    {
        fitstTime = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (fitstTime)
        {
            fitstTime = false;

            Debug.Log("Colliding: " + type);

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
}
