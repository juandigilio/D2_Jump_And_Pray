using UnityEngine;

public class AnimationsEventManager : MonoBehaviour
{
    public void TriggerRollFinished()
    {
        EventManager.Instance.TriggerRollFinished();
    }

    public void TriggerAnimationFinished()
    {
        EventManager.Instance.TriggerAnimationFinished();
    }

    public void TriggerResetPlayer()
    {
        EventManager.Instance.TriggerResetPlayer();
    }

    public void TriggerResetGame()
    {
        EventManager.Instance.TriggerResetGame();
    }
}
