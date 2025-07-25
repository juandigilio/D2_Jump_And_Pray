using UnityEngine;
using System.Collections;


public class LevelConection : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform activatedTarget;
    [SerializeField] private Transform nextLevelTarget;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Collider exitZone;
    [SerializeField] private float moeUpDuration = 3f;
    [SerializeField] private float nextLevelDuration = 6f;
    [SerializeField] private float animationPause = 1f;
    [SerializeField] private bool isLastLevel = false;

    private bool isActivated = false;
    private bool hasMoved = false;


    public void ChangeLevel()
    {
        if (isActivated && !hasMoved)
        {
            StartCoroutine(MoveRoutine(nextLevelTarget));
            levelManager.TurnOffLevel();
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

            if (isLastLevel)
            {
                SceneManager.LoadWiningScene();
            }
            else
            {
                SceneManager.LoadNextSceneAsync();
            }

            levelManager.TurnOffLevel();
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
