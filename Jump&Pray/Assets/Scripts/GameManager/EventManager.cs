using System;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    //public event Action OnDoorOpen;
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

    public void TriggerCinematicFinished()
    {
        OnCinematicEnded?.Invoke();
    }

    public void TriggerDoorOpen(Vector3 position, Vector3 target)
    {
        //OnDoorOpen?.Invoke();
        OnCinematicStarted?.Invoke(position, target);
    }
}
