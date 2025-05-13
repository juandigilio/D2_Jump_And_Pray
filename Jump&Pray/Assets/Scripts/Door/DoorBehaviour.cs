using System;
using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float doorHeight = 4f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float animationPause = 1f;

    private Cameraman cameraman;
    private bool isOpen = false;


    private void Start()
    {
        cameraman = GameManager.Instance.GetCameraman();
    }


    private void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Door opened");    
            StartCoroutine(MoveUpRoutine());
        }       
    }

    private IEnumerator MoveUpRoutine()
    {
        EventManager.Instance.TriggerCinematicStarted(cameraPosition.position, target.position);

        isOpen = true;

        Vector3 initialPosition = door.transform.position;
        Vector3 targetPosition = door.transform.position + Vector3.up * doorHeight;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            door.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //door.transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        EventManager.Instance.TriggerCinematicFinished();
    }
}
