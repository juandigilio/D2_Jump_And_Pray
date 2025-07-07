using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnAnimationStarted;
    public event Action OnAnimationFinished;

    public event Action<Vector3, GameObject> OnCinematicStarted;
    public event Action OnCinematicFinished;

    public event Action OnCinematicFallStarted;
    public event Action OnCinematicFallFinished;

    public event Action OnPlayerJumped;
    public event Action OnPlayerDied;
    public event Action OnPlayerLanded;
    public event Action OnPlayerRolled;
    public event Action OnRollFinished;

    public event Action OnPlayerWon;
    public event Action OnPlayerLost;

    public event Action<Vector3> OnMenuLoaded;

    public event Action OnShowOptionsMenu;
    public event Action OnHideOptionsMenu;


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

    public void TriggerCinematicFallStarted()
    {
        OnCinematicFallStarted?.Invoke();
        OnAnimationStarted?.Invoke();
    }

    public void TriggerCinematicFallFinished()
    {
        OnCinematicFallFinished?.Invoke();
    }

    public void TriggerPlayerJump()
    {
        OnPlayerJumped?.Invoke();
    }

    public void TriggerPlayerDied()
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

    public void TriggerMenuLoaded(Vector3 startPos)
    {
        OnMenuLoaded?.Invoke(startPos);
    }

    public void TriggerShowOptionsMenu()
    {
        OnShowOptionsMenu?.Invoke();
    }

    public void TriggerHideOptionsMenu()
    {
        OnHideOptionsMenu?.Invoke();
    }

    public void TriggerPlayerWon()
    {
        OnPlayerWon?.Invoke();
        OnAnimationStarted?.Invoke();
    }

    public void TriggerPlayerLost()
    {
        OnPlayerLost?.Invoke();
    }
}
