using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnAnimationStarted;
    public event Action OnAnimationFinished;

    public event Action<Vector3, GameObject> OnCinematicStarted;
    public event Action OnCinematicFinished;

    public event Action OnPlayerJumped;


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

    public void TriggerCinematicStarted(Vector3 cameraPosition, GameObject target)
    {
        OnCinematicStarted?.Invoke(cameraPosition, target);
    }

    public void TriggerCinematicFinished()
    {
        OnCinematicFinished?.Invoke();
    }

    public void TriggerPlayerJump()
    {
        OnPlayerJumped?.Invoke();
    }
}
