using System.Collections;
using UnityEngine;

public class ArcadeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject machine;
    [SerializeField] private float dropDuration = 1f;
    [SerializeField] private float dropHeight = 2f;
    [SerializeField] private float forwardOffset = 1f;
    [SerializeField] private float UpOffset = 1f;
    [SerializeField] private float RightOffset = 1f;


    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isDropping;


    private void Awake()
    {
        if (machine == null)
        {
            Debug.LogError("Arcade machine GameObject is not assigned.");
        }

        machine.SetActive(false);
        isDropping = false;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnShowOptionsMenu += StartDroppingArcade;
        EventManager.Instance.OnHideOptionsMenu += StartQuitingArcade;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnShowOptionsMenu -= StartDroppingArcade;
        EventManager.Instance.OnHideOptionsMenu -= StartQuitingArcade;
    }

    private void StartDroppingArcade()
    {
        if (!isDropping)
        {
            StartCoroutine(DropArcadeMachine());
        }
    }

    private void StartQuitingArcade()
    {
        if (!isDropping)
        {
            StartCoroutine(QuitArcadeMachine());
        }
    }

    private void SetTargestPositions()
    {
        endPosition = GameManager.Instance.GetCameraman().transform.position;

        endPosition += GameManager.Instance.GetCameraman().transform.forward * forwardOffset;
        endPosition += GameManager.Instance.GetCameraman().transform.up * UpOffset;
        endPosition += GameManager.Instance.GetCameraman().transform.right * RightOffset;

        startPosition = endPosition;
        startPosition += GameManager.Instance.GetCameraman().transform.up * dropHeight;

        machine.transform.position = startPosition;
        machine.transform.LookAt(GameManager.Instance.GetCameraman().transform.position);
    }

    private IEnumerator DropArcadeMachine()
    {
        isDropping = true;
        machine.SetActive(true);

        SetTargestPositions();

        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration;
            machine.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        machine.transform.position = endPosition;

        isDropping = false;
        Time.timeScale = 0f;
    }

    private IEnumerator QuitArcadeMachine()
    {
        isDropping = true;

        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration;
            machine.transform.position = Vector3.Lerp(endPosition, startPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        machine.transform.position = startPosition;
        machine.SetActive(false);

        isDropping = false;
    }

    public bool IsDropping()
    {
        return isDropping;
    }
}
