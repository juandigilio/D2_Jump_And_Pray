using UnityEngine;

public class CorridorEnd : MonoBehaviour
{
    private Cameraman cameraman;


    private void Start()
    {
        cameraman = GameManager.Instance.GetCameraman();
        if (cameraman == null)
        {
            Debug.LogError("Cameraman not found in GameManager!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraman.SetCameraMode(CameraMode.ThirdPerson);
        }
    }
}
