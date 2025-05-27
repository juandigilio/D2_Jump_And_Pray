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
    public event Action OnPlayerDied;
    public event Action OnPlayerLanded;
    public event Action OnPlayerRolled;
    public event Action OnRollFinished;


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

    public void TriggerPlayerDeath()
    {
        OnPlayerDied?.Invoke();
    }

    public void TriggerPlayerLanded()
    {
        OnPlayerLanded?.Invoke();
    }

    public void TriggerPlayerRolled()
    {
        OnPlayerRolled?.Invoke();
    }

    public void TriggerRollFinished()
    {
        OnRollFinished?.Invoke();
    }
}
