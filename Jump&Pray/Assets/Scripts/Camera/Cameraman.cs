using UnityEngine;

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private float cameraRotation;


    private void Start()
    {
        InitCamera();
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void InitCamera()
    {
        mainCamera = Camera.main.GetComponent<Camera>();

        mainCamera.transform.position = target.position + offset;
        mainCamera.transform.LookAt(target);
    }

    private void UpdatePosition()
    {
        Quaternion newRotation = Quaternion.Euler(0, cameraRotation, 0);
        Vector3 rotatedOffset = newRotation * offset;
        rotatedOffset += target.position;

        mainCamera.transform.position = rotatedOffset;

        mainCamera.transform.LookAt(target);
    }

    public void UpdateInputRotation(Vector2 input)
    {

    }
}
