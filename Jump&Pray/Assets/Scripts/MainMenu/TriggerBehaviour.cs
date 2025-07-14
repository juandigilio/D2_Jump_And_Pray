using UnityEngine;


public class TriggerBehaviour : MonoBehaviour
{
    public enum TriggerType { Play, Instructions, Unload, Exit }
    public TriggerType type;

    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Transform cameraPositon;
    [SerializeField] private Transform camerTarget;

    private Cameraman cameraman;

    private bool fitstTime;

    private void Awake()
    {
        fitstTime = true;
    }

    private void Start()
    {
        cameraman = GameManager.Instance.GetCameraman();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
                    case TriggerType.Instructions:
                    {
                        cameraman.LockCamera(cameraPositon.position, camerTarget.position);
                        fitstTime = true;
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

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraman.UnlockCamera();
        }
    }
}
