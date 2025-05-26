using System.Collections.Generic;
using UnityEngine;

public class LinearPlatformMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loopContinuously = false;
    [SerializeField] private bool waitAtEndUntilPlayerLeaves = true;

    private int currentTargetIndex = 0;
    private bool movingForward = true;
    private bool isActive = false;
    private bool playerOnPlatform = false;

    private void Start()
    {
        if (targets == null || targets.Count < 2)
        {
            Debug.LogError("La plataforma necesita al menos 2 puntos en 'targets'.");
            enabled = false;
        }

        isActive = loopContinuously;
    }

    private void Update()
    {
        if (!isActive) return;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Transform target = targets[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            HandleArrivalAtTarget();
        }
    }

    private void HandleArrivalAtTarget()
    {
        if (movingForward)
        {
            if (currentTargetIndex < targets.Count - 1)
            {
                currentTargetIndex++;
            }
            else
            {
                if (waitAtEndUntilPlayerLeaves && playerOnPlatform)
                {
                    isActive = false;
                }
                else
                {
                    movingForward = false;
                    currentTargetIndex--;
                }
            }
        }
        else
        {
            if (currentTargetIndex > 0)
            {
                currentTargetIndex--;
            }
            else
            {
                isActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log("Player entered the platform trigger");

        playerOnPlatform = true;

        if (!isActive && (!loopContinuously || currentTargetIndex == 0))
        {
            isActive = true;
            movingForward = true;
            currentTargetIndex = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerOnPlatform = false;

        if (!loopContinuously && !movingForward)
        {
            isActive = false;
        }
    }
}
