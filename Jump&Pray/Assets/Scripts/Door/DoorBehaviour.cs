using System;
using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float doorOpenSpeed = 2f;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform target;

    private Camera mainCamera;
    private bool isOpen = false;

    public static Action OnCinematicStarted;
    public static Action OnCinematicEnded;

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Door opened");
            isOpen = true;
            
        }
        
    }

    private IEnumerator MoveUpRoutine()
    {
        QuestCamera.SetCamera(cameraPoint, mainCamera, cameraHeight, offsetZ);

        initialPosition = grate.transform.position;
        Vector3 targetPosition = grate.transform.position + Vector3.up * doorHeight;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            //Debug.Log("initialPosition: " + initialPosition);

            grate.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        grate.transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.enabled = true;
        cameraman.enabled = true;
        isAnimating = false;

        OnDoorAnimationFinished?.Invoke();
    }
}
