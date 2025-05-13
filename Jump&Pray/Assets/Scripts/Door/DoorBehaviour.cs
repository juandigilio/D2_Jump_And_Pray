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

    private bool isOpen = false;

    private void OnEnable()
    {
        //EventManager.Instance.OnDoorOpen += OpenDoor;
    }

    private void OnDisable()
    {
        //EventManager.Instance.OnDoorOpen -= OpenDoor;
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            EventManager.Instance.TriggerDoorOpen(cameraPosition.position, target.position);

            StartCoroutine(MoveUpRoutine());
        }       
    }

    private IEnumerator MoveUpRoutine()
    {
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

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        EventManager.Instance.TriggerCinematicFinished();
    }
}
