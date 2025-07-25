using UnityEngine;

public class RollingCrusher : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private MeshCollider meshCollider;
    private Vector3 initialPosition;
    private bool hasLanded = false;
    private bool hasSmashed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing!");
        }
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            Debug.LogError("MeshCollider missing!");
        }

        rb.isKinematic = true;
        meshCollider.isTrigger = false;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (hasLanded && !hasSmashed)
        {
            RotateAndMove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasLanded && other.CompareTag("Activator"))
        {
            hasLanded = true;
            rb.isKinematic = true;
            meshCollider.isTrigger = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                hasSmashed = true;
                EventManager.Instance.TriggerPlayerSmashed();
            }
            else if (other.CompareTag("Target"))
            {
                hasSmashed = true;
            }
        }
    }

    private void RotateAndMove()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void ActivateRoller()
    {
        rb.isKinematic = false;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        rb.isKinematic = true;
        hasLanded = false;
        meshCollider.isTrigger = false;
        hasSmashed = false;
    }
}
