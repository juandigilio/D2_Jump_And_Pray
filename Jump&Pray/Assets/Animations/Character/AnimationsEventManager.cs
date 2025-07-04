using UnityEngine;

public class AnimationsEventManager : MonoBehaviour
{
    public void TriggerAnimationFinished()
    {
        EventManager.Instance.TriggerAnimationFinished();
    }
}
