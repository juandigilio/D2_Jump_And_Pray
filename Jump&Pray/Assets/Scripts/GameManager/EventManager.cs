using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnCinematicStarted;
    public event Action OnCinematicEnded;

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

    public void TriggerCinematicStarted()
    {
        OnCinematicStarted?.Invoke();
    }
}
