using Unity.VisualScripting;
using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    FirstPerson,
    Corridor,
}

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 thirdPersonOffset;
    [SerializeField] private Vector3 firstPersonOffset;
    [SerializeField] private Vector3 corridorOffset;
    [SerializeField] private float minVerticalAngle = -60f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private Camera mainCamera;
    private CameraMode cameraMode;
    private Vector2 inputRotation;
    private Vector2 cameraRotation;
    private Vector3 corridorTarget;
    private Vector3 corridorEnd;
    private float corridorDistance;


    private void Start()
    {
        InitCamera();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (cameraMode == CameraMode.ThirdPerson)
        {
            UpdateThirdPersonCamera();
        }
        else if (cameraMode == CameraMode.FirstPerson)
        {
            UpdateFirstPersonCamera();
        }
        else if (cameraMode == CameraMode.Corridor)
        {
            UpdateCorridorCamera();
        }
    }

    private void UpdateThirdPersonCamera()
    {
        mainCamera.transform.position = CalculateThirdPersonPosition();
        mainCamera.transform.LookAt(target);
    }

    private void UpdateFirstPersonCamera()
    {
        mainCamera.transform.position = target.position + firstPersonOffset;
        mainCamera.transform.rotation = CalculateFirstPersonRotation();
    }

    private void UpdateCorridorCamera()
    {
        Vector3 direction = (corridorTarget - mainCamera.transform.position).normalized;
        Vector3 targetPosition = corridorTarget - direction * corridorDistance;

        float distanceToEnd = Vector3.Distance(corridorTarget, corridorEnd);
        float distanceToCamera = Vector3.Distance(corridorTarget, targetPosition);

        if (distanceToCamera > distanceToEnd)
        {
            targetPosition = corridorEnd;
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 5f);
        mainCamera.transform.LookAt(corridorTarget);
    }

    private Vector3 CalculateThirdPersonPosition()
    {
        NormalizeRotation();

        Quaternion newRotation = Quaternion.Euler(0, cameraRotation.x, 0);

        Vector3 rotatedOffset = newRotation * thirdPersonOffset;

        return rotatedOffset + target.position;
    }

    private Quaternion CalculateFirstPersonRotation()
    {
        NormalizeRotation();
        return Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);

    }

    private void InitCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        cameraMode = CameraMode.ThirdPerson;
        mainCamera.transform.position = target.position + thirdPersonOffset;
        mainCamera.transform.LookAt(target);
    }

    private void NormalizeRotation()
    {
        if (cameraMode == CameraMode.FirstPerson)
        {
            cameraRotation += inputRotation * PlayerConfig.firstPersonMultiplier;
            cameraRotation.y = Mathf.Clamp(cameraRotation.y, minVerticalAngle, maxVerticalAngle);
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -180, 180);
        }
        else if (cameraMode == CameraMode.ThirdPerson)
        {
            cameraRotation.x += inputRotation.x * PlayerConfig.thirdPersonMultiplier;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -180, 180);
        }       
    }
   
    public void UpdateInputRotation(Vector2 input)
    {
        inputRotation = input;
    }

    public void SetCorridorCamera(Transform target, Transform end, float distance)
    {
        corridorTarget = target.position;
        corridorEnd = end.position;
        corridorDistance = distance;
        cameraMode = CameraMode.Corridor;
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }
}
