using UnityEngine;
using UnityEngine.InputSystem;


public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }
    }

    private void Update()
    {

    }

    public void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody not found on the GameObject.");
        }
    }
}
