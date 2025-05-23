using UnityEngine;
using System.Collections;


public class LevelConection : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform activatedTarget;
    [SerializeField] private Transform nextLevelTarget;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Collider exitZone;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float animationPause = 1f;

    private bool isActivated = false;
    private bool hasMoved = false;


    public void ChangeLevel()
    {
        if (isActivated && !hasMoved)
        {
            EventManager.Instance.TriggerLoadNextLevel();
            StartCoroutine(MoveRoutine(nextLevelTarget));
        }       
    }

    public void ActivatePlatform()
    {
        Debug.Log("Activating platform");
        if (!isActivated)
        {
            StartCoroutine(MoveRoutine(activatedTarget));
        }
    }

    private IEnumerator MoveRoutine(Transform target)
    {
        Debug.Log("Moving platform to: " + target.position);
        if (isActivated)
        {
            EventManager.Instance.TriggerAnimationStarted();
            hasMoved = true;
        }
        else
        {
            EventManager.Instance.TriggerCinematicStarted(cameraPosition.position, platform);
        }      

        Vector3 initialPosition = platform.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            platform.transform.position = Vector3.Lerp(initialPosition, target.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isActivated)
        {
            EventManager.Instance.TriggerAnimationFinished();
            EventManager.Instance.TriggerUnloadLastLevel();
        }
        else
        {
            EventManager.Instance.TriggerCinematicFinished();
            isActivated = true;
        }
    }

    //private IEnumerator GoToNextLevelCoroutine()
    //{
    //    EventManager.Instance.TriggerAnimationStarted();

    //    Vector3 initialPosition = platform.position;

    //    float elapsedTime = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        platform.position = Vector3.Lerp(initialPosition, nextLevelTarget.position, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    elapsedTime = 0f;

    //    while (elapsedTime < animationPause)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    EventManager.Instance.TriggerAnimationFinished();
    //}
}
