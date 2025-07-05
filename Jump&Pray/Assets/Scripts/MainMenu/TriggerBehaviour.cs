using UnityEngine;


public class TriggerBehaviour : MonoBehaviour
{
    public enum TriggerType { Play, Unload, Exit }
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
                case TriggerType.Exit:
                {
                    menuManager.QuitGame();
                    break;
                }
            }
        } 
    }
}
