using UnityEngine;

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private float inputRotation;
    private float cameraRotation;


    private void Start()
    {
        InitCamera();
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        mainCamera.transform.position = CalculateNewPosition();

        mainCamera.transform.LookAt(target);
    }

    public void UpdateInputRotation(Vector2 input)
    {
        inputRotation = input.x;

        Debug.Log(inputRotation);
    }

    private void NormalizeRotation()
    {
        cameraRotation += inputRotation;

        if (cameraRotation > 360)
        {
            cameraRotation -= 360;
        }
        else if (cameraRotation < 0)
        {
            cameraRotation += 360;
        }
    }

    private Vector3 CalculateNewPosition()
    {
        NormalizeRotation();

        Quaternion newRotation = Quaternion.Euler(0, cameraRotation, 0);

        Vector3 rotatedOffset = newRotation * offset;
        rotatedOffset += target.position;

        return rotatedOffset;
    }

    private void InitCamera()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        mainCamera.transform.position = target.position + offset;
        mainCamera.transform.LookAt(target);
    }
}
