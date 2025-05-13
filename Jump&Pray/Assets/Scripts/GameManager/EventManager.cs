using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action<Vector3, Vector3> OnCinematicStarted;
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

    public void TriggerCinematicStarted(Vector3 position, Vector3 target)
    {
        OnCinematicStarted?.Invoke(position, target);
    }

    public void TriggerCinematicFinished()
    {
        OnCinematicEnded?.Invoke();
    }
}
