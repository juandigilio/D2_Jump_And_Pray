using UnityEngine;

public class RailEnd : MonoBehaviour
{
    [SerializeField] private GameObject cart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.TriggerPlayerStoppedDriving();
            cart.SetActive(false);
        }
    }
}
