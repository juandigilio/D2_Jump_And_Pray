using Unity.VisualScripting;
using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    FirstPerson,
    Corridor,
    Cinematic,
    Locked,
    Victory
}

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Vector3 thirdPersonOffset;
    [SerializeField] private Vector3 firstPersonOffset;
    [SerializeField] private Vector3 corridorOffset;
    [SerializeField] private float minVerticalAngle = -60f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float corridorDistance = 1;
    [SerializeField] private float victoryOrbitSpeed = 20f;
    [SerializeField] private float victoryOrbitDistance = 4f;
    [SerializeField] private float victoryOrbitHeight = 2f;

    private Camera mainCamera;
    private Transform target;
    private CameraMode cameraMode;
    private Vector2 inputRotation;
    private Vector2 cameraRotation;
    private GameObject cinematicTarget;
    private Vector3 corridorStart;
    private Vector3 corridorEnd;
    private float victoryOrbitAngle = 0f;


    private void OnEnable()
    {
        GameManager.Instance.RegisterCameraman(this);

        EventManager.Instance.OnMenuLoaded += SetThirdPersonCamera;
        EventManager.Instance.OnPlayerWon += SetVictoryCamera;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnMenuLoaded -= SetThirdPersonCamera;
        EventManager.Instance.OnPlayerWon -= SetVictoryCamera;
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
        else if (cameraMode == CameraMode.Victory)
        {
            UpdateVictoryCamera();
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

    private void UpdateVictoryCamera()
    {
        victoryOrbitAngle += victoryOrbitSpeed * Time.deltaTime;
        float rad = victoryOrbitAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Sin(rad) * victoryOrbitDistance, victoryOrbitHeight, Mathf.Cos(rad) * victoryOrbitDistance);
        mainCamera.transform.position = target.position + offset;

        mainCamera.transform.LookAt(target.position + Vector3.up);
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

    public void SetVictoryCamera()
    {
        cameraMode = CameraMode.Victory;
        victoryOrbitAngle = 0f;
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }

    public void LockCamera(Vector3 cameraPosition, Vector3 target)
    {
        mainCamera.transform.position = cameraPosition;
        mainCamera.transform.LookAt(target);

        cameraMode = CameraMode.Locked;
    }

    public bool IsCameraLocked()
    {
        return cameraMode == CameraMode.Locked;
    }

    public void UnlockCamera()
    {
        cameraMode = CameraMode.ThirdPerson;
    }
}
