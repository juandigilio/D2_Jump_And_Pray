using Dreamteck.Splines;
using UnityEngine;

public class RailConector : MonoBehaviour
{
    [SerializeField] private SplineFollower follower;
    [SerializeField] private SplineComputer rail;
    [SerializeField] private RailSwitcher railSwitcher;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Switch();
        }
    }

    private void Switch()
    {
        follower.spline = rail;
        follower.SetPercent(0);

        railSwitcher.SetActive();
    }
}
