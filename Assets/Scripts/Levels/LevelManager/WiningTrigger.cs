using UnityEngine;

public class WiningTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.TriggerPlayerWon();
        }
    }
}
