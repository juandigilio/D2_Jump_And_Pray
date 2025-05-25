using UnityEngine;

public class FollowPlatform : MonoBehaviour
{
    private PlayerController playerController;
    //private Rigidbody rigidbody;
    private CapsuleCollider collider;
    private Vector3 groundPosition;
    private Vector3 lastGroundPosition;
    private int groundID;
    private int lastGroundID;
    private float height;
    private float width;
    private Quaternion currentRotation;
    private Quaternion lastRotation;

    [SerializeField] private float gizmoLength;
    private Vector3 gizmoOrigin;
    private Vector3 gizmoDirection;
    private float gizmoRadius;

    private bool isGroundedGizmo;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found");
        }

        collider = gameObject.GetComponent<CapsuleCollider>();
        GetObjectSize();
    }

    void Update()
    {
        UpdateGizmoOrigin();

     
        CheckGround();
    }

    private void UpdateGizmoOrigin()
    {
        gizmoOrigin = collider.bounds.center + transform.up * (collider.bounds.extents.y);
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.SphereCast(gizmoOrigin, gizmoRadius, gizmoDirection, out hit, gizmoLength))
        {
            GameObject groundedObject = hit.collider.gameObject;

            groundID = groundedObject.GetInstanceID();
            groundPosition = groundedObject.transform.position;
            currentRotation = groundedObject.transform.rotation;

            isGroundedGizmo = true;

            if (groundID == lastGroundID)
            {
                UpdateGroundMovement();
                UpdateGroundRotation(groundedObject);
            }

            StoreLastGroundData();
        }
        else
        {
            isGroundedGizmo = false;
            ResetGroundState();
        }
    }

    private void UpdateGroundMovement()
    {
        Vector3 platformDelta = groundPosition - lastGroundPosition;

        if (platformDelta != Vector3.zero)
        {
            transform.position += platformDelta;
        }
    }

    private void UpdateGroundRotation(GameObject ground)
    {
        if (currentRotation != lastRotation)
        {
            Vector3 rotationDelta = (currentRotation.eulerAngles - lastRotation.eulerAngles);
            transform.RotateAround(ground.transform.position, Vector3.up, rotationDelta.y);
        }
    }

    private void StoreLastGroundData()
    {
        lastGroundID = groundID;
        lastGroundPosition = groundPosition;
        lastRotation = currentRotation;
    }

    private void ResetGroundState()
    {
        isGroundedGizmo = false;
        lastGroundID = -999;
        lastGroundPosition = Vector3.zero;
        lastRotation = Quaternion.identity;
    }

    private void GetObjectSize()
    {
        if (collider != null)
        {
            width = collider.bounds.size.x;
            height = collider.bounds.size.y;
            gizmoDirection = -transform.up;
            gizmoRadius = width / 2;
            gizmoLength = (collider.bounds.size.y - gizmoRadius) * 1.05f;
        }
        else
        {
            Debug.LogError("El GameObject no tiene un BoxCollider.");
        }
    }

    private void OnDrawGizmos()
    {
        if (isGroundedGizmo)
        {
            Gizmos.color = Color.cyan;
            Debug.Log("Gizmo is grounded");
        }
        else
        {
            Debug.Log("Gizmo is not grounded");
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(gizmoOrigin, gizmoOrigin + gizmoDirection * gizmoLength);
        Gizmos.DrawWireSphere(gizmoOrigin, gizmoRadius);
        Gizmos.DrawWireSphere(gizmoOrigin + gizmoDirection * gizmoLength, gizmoRadius);
    }
}
