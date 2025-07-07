using UnityEngine;

public class CorridorStart : MonoBehaviour
{
    [SerializeField] private RollingCrusher rollingCrusher;
    [SerializeField] private Transform corridorEndPosition;

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
            cameraman.SetCorridorCamera(transform.position, corridorEndPosition.position);
            rollingCrusher.ActivateRoller();
        }
    }
}
