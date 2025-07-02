using UnityEngine;
using System.Collections;


public class LevelConection : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform activatedTarget;
    [SerializeField] private Transform nextLevelTarget;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Collider exitZone;
    [SerializeField] private float moeUpDuration = 3f;
    [SerializeField] private float nextLevelDuration = 6f;
    [SerializeField] private float animationPause = 1f;

    private bool isActivated = false;
    private bool hasMoved = false;


    public void ChangeLevel()
    {
        if (isActivated && !hasMoved)
        {
            StartCoroutine(MoveRoutine(nextLevelTarget));
        }       
    }

    public void ActivatePlatform()
    {
        if (!isActivated)
        {
            StartCoroutine(MoveRoutine(activatedTarget));
        }
    }

    private IEnumerator MoveRoutine(Transform target)
    {
        float duration;

        if (isActivated)
        {
            hasMoved = true;
            duration = nextLevelDuration;

            EventManager.Instance.TriggerAnimationStarted();
            SceneManager.LoadNextSceneAsync();
        }
        else
        {
            duration = moeUpDuration;

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
        }
        else
        {
            isActivated = true;
            EventManager.Instance.TriggerCinematicFinished();
        }
    }
}
