using UnityEngine;

public class CinematicTarget : MonoBehaviour
{
    [SerializeField] private CinematicFall cinematicFall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cinematicFall.StopCinematicFall();
        }
    }
}
