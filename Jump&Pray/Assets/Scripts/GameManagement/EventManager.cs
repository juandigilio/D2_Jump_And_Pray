using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnAnimationStarted;
    public event Action OnAnimationFinished;

    public event Action<Vector3, Vector3> OnCinematicStarted;
    public event Action OnCinematicFinished;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TriggerAnimationStarted()
    {
        OnAnimationStarted?.Invoke();
    }

    public void TriggerAnimationFinished()
    {
        OnAnimationFinished?.Invoke();
    }

    public void TriggerCinematicStarted(Vector3 cameraPosition, Vector3 target)
    {
        OnCinematicStarted?.Invoke(cameraPosition, target);
    }

    public void TriggerCinematicFinished()
    {
        OnCinematicFinished?.Invoke();
    }

}
