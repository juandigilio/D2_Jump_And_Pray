using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private ArcadeBehaviour arcadeBehaviour;

    private bool isPaused;

    private void Start()
    {
        GameManager.Instance.RegisterOptionsManager(this);

        isPaused = false;
    }

    public void ShowOptions()
    {
        if (!arcadeBehaviour.IsDropping())
        {       
            if (!isPaused)
            {
                isPaused = true;
                EventManager.Instance.TriggerShowOptionsMenu();
            }
            else
            {
                isPaused = false;
                EventManager.Instance.TriggerHideOptionsMenu();
            }
        }
    }
}
