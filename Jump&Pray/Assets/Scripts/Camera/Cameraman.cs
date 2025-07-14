using Unity.VisualScripting;
using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    FirstPerson,
    Corridor,
    Cinematic,
    Locked
}

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Vector3 thirdPersonOffset;
    [SerializeField] private Vector3 firstPersonOffset;
    [SerializeField] private Vector3 corridorOffset;
    [SerializeField] private float minVerticalAngle = -60f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float corridorDistance = 1;

    private Camera mainCamera;
    private Transform target;
    private CameraMode cameraMode;
    private Vector2 inputRotation;
    private Vector2 cameraRotation;
    private GameObject cinematicTarget;
    private Vector3 corridorStart;
    private Vector3 corridorEnd;


    private void OnEnable()
    {
        GameManager.Instance.RegisterCameraman(this);

        EventManager.Instance.OnMenuLoaded += SetThirdPersonCamera;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnMenuLoaded -= SetThirdPersonCamera;
    }

    private void Start()
    {
        InitCamera();
    }

    public void UpdateCamera()
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
        else if (cameraMode == CameraMode.Cinematic)
        {
            UpdateCinematicCamera();
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
        Vector3 rielStart = corridorStart;
        Vector3 rielEnd = corridorEnd;
        Vector3 player = target.position;

        Vector3 rielDir = (rielEnd - rielStart).normalized;
        Vector3 toPlayer = player - rielStart;
        float projected = Vector3.Dot(toPlayer, rielDir);
        float totalDist = Vector3.Distance(rielStart, rielEnd);

        float clampedProj = Mathf.Clamp(projected - corridorDistance, 0f, totalDist);
        Vector3 camPosition = rielStart + rielDir * clampedProj;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camPosition, Time.deltaTime * 5f);

        Quaternion targetRotation = Quaternion.LookRotation(player - mainCamera.transform.position);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void UpdateCinematicCamera()
    {
        mainCamera.transform.LookAt(cinematicTarget.transform.position);
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
        target = GameManager.Instance.GetPlayerController().GetTransform();

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

            NormalizeX();
        }
        else if (cameraMode == CameraMode.ThirdPerson)
        {
            cameraRotation.x += inputRotation.x * PlayerConfig.thirdPersonMultiplier;

            NormalizeX();
        }       
    }

    private void NormalizeX()
    {
        if (cameraRotation.x > 360)
        {
            cameraRotation.x -= 360;
        }
        else if (cameraRotation.x < -360)
        {
            cameraRotation.x += 360;
        }
    }

    public void UpdateInputRotation(Vector2 input)
    {
        inputRotation = input;
    }

    public void SetFirstPersonCamera()
    {
        cameraMode = CameraMode.FirstPerson;
    }

    public void SetThirdPersonCamera()
    {
        cameraMode = CameraMode.ThirdPerson;
    }

    private void SetThirdPersonCamera(Vector3 v)
    {
        SetThirdPersonCamera();
    }

    public void SetCorridorCamera(Vector3 start, Vector3 end)
    {
        corridorStart = start;
        corridorEnd = end;
        cameraMode = CameraMode.Corridor;
    }

    public void SetCinematicCamera(Vector3 cameraPosition, GameObject target)
    {
        cinematicTarget = target;

        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.LookAt(cinematicTarget.transform.position);

        cameraMode = CameraMode.Cinematic;
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }

    public void LockCamera(Vector3 cameraPosition, Vector3 target)
    {
        Debug.Log("Locking camera...");
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.LookAt(target);

        cameraMode = CameraMode.Locked;
    }

    public void UnlockCamera()
    {
        Debug.Log("Unlocking camera...");
        cameraMode = CameraMode.ThirdPerson;
    }
}
