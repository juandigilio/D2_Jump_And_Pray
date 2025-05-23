using UnityEngine;

public class FollowPlatform : MonoBehaviour
{
    private Transform parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent = other.transform.parent;

            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(parent);
        }
    }
}
