using Unity.VisualScripting;
using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    FirstPerson,
    Corridor,
    Cinematic,
    Driving,
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

    private Vector3 lockTargetPosition;
    private Vector3 lockLookAtTarget;
    private Vector3 lockStartPosition;
    private Quaternion lockStartRotation;
    private bool isLockingCamera = false;
    private float lockLerpTime = 1f;
    private float lockLerpTimer = 0f;

    private bool isUnlockingCamera = false;
    private float unlockLerpTime = 1f;
    private float unlockLerpTimer = 0f;
    private Vector3 unlockStartPosition;
    private Quaternion unlockStartRotation;


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
        if (LockingCamera() || UnlockingCamera())
        {
            return;
        }

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
        else if (cameraMode == CameraMode.Driving)
        {
            UpdateDrivingCamera();
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

    private void UpdateDrivingCamera()
    {
        mainCamera.transform.position =  GameManager.Instance.GetRailSwitcher().GetCameraPosition();

        mainCamera.transform.LookAt(target);
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

    private bool LockingCamera()
    {
        if (isLockingCamera)
        {
            lockLerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(lockLerpTimer / lockLerpTime);

            Vector3 newPos = Vector3.Lerp(lockStartPosition, lockTargetPosition, t);
            Quaternion newRot = Quaternion.Slerp(lockStartRotation, Quaternion.LookRotation(lockLookAtTarget - newPos), t);

            mainCamera.transform.position = newPos;
            mainCamera.transform.rotation = newRot;

            if (t >= 1f)
            {
                isLockingCamera = false;
                cameraMode = CameraMode.Locked;
            }
            return true;
        }
        return false;
    }

    private bool UnlockingCamera()
    {
        if (isUnlockingCamera)
        {
            unlockLerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(unlockLerpTimer / unlockLerpTime);

            Vector3 targetPosition = CalculateThirdPersonPosition();
            Quaternion targetRotation = Quaternion.LookRotation(target.position - targetPosition);

            Vector3 newPos = Vector3.Lerp(unlockStartPosition, targetPosition, t);
            Quaternion newRot = Quaternion.Slerp(unlockStartRotation, targetRotation, t);

            mainCamera.transform.position = newPos;
            mainCamera.transform.rotation = newRot;

            if (t >= 1f)
            {
                isUnlockingCamera = false;
                cameraMode = CameraMode.ThirdPerson;
            }

            return true;
        }
        return false;
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

    public void SetThirdPersonCamera(Vector3 v)
    {
        SetThirdPersonCamera();
    }

    public void SetCorridorCamera(Vector3 start, Vector3 end)
    {
        corridorStart = start;
        corridorEnd = end;
        cameraMode = CameraMode.Corridor;
    }

    public void SetDrivingCamera()
    {
        cameraMode = CameraMode.Driving;
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
        lockTargetPosition = cameraPosition;
        lockLookAtTarget = target;

        lockLerpTimer = 0f;
        isLockingCamera = true;

        lockStartPosition = mainCamera.transform.position;
        lockStartRotation = mainCamera.transform.rotation;
    }

    public void UnlockCamera()
    {
        isUnlockingCamera = true;
        unlockLerpTimer = 0f;

        unlockStartPosition = mainCamera.transform.position;
        unlockStartRotation = mainCamera.transform.rotation;
    }

    public bool IsCameraLocked()
    {
        return cameraMode == CameraMode.Locked;
    }
}
