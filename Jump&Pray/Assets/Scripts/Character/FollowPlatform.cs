using UnityEngine;
using System.Collections.Generic;

public class FollowPlatform : MonoBehaviour
{
    [SerializeField] private float gizmoLength;

    private PlayerController playerController;
    private CapsuleCollider playerCollider;
    private Vector3 groundPosition;
    private Vector3 lastGroundPosition;
    private int groundID;
    private int lastGroundID;
    private float width;
    private Quaternion currentRotation;
    private Quaternion lastRotation;

    private Vector3 gizmoOrigin;
    private Vector3 gizmoDirection;
    private float gizmoRadius;

    private bool isGroundedGizmo;

    private GameObject lastGroundObject = null;

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

    void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found", this);
        }

        playerCollider = GetComponent<CapsuleCollider>();
        GetObjectSize();
    }

    void Update()
    {
        UpdateGizmoOrigin();
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        if (isGroundedGizmo)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(gizmoOrigin, gizmoOrigin + gizmoDirection * gizmoLength);
        Gizmos.DrawWireSphere(gizmoOrigin, gizmoRadius);
        Gizmos.DrawWireSphere(gizmoOrigin + gizmoDirection * gizmoLength, gizmoRadius);
    }

    private void UpdateGizmoOrigin()
    {
        gizmoOrigin = playerCollider.bounds.center + transform.up * (playerCollider.bounds.extents.y);
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.SphereCast(gizmoOrigin, gizmoRadius, gizmoDirection, out hit, gizmoLength, ~0, QueryTriggerInteraction.Ignore))
        {
            GameObject groundedObject = hit.collider.gameObject;

            if (!groundedObject.CompareTag("Smasher"))
            {
                groundID = groundedObject.GetInstanceID();
                groundPosition = groundedObject.transform.position;
                currentRotation = groundedObject.transform.rotation;

                isGroundedGizmo = true;

                if (lastGroundObject != null && lastGroundObject != groundedObject)
                {
                    RestoreMaterial(lastGroundObject);
                }

                Renderer renderer = groundedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (!originalMaterials.ContainsKey(groundedObject))
                    {
                        originalMaterials[groundedObject] = renderer.material;
                        renderer.material = new Material(renderer.material);
                    }

                    renderer.material.color = Color.red;
                }

                lastGroundObject = groundedObject;

                if (groundID == lastGroundID)
                {
                    UpdateGroundMovement();
                    UpdateGroundRotation(groundedObject);
                    Debug.Log("Player is grounded on the same platform: " + groundedObject.name);
                }

                StoreLastGroundData();
            }
        }
        else
        {
            isGroundedGizmo = false;
            ResetGroundState();
        }

        Debug.DrawRay(gizmoOrigin, gizmoDirection * gizmoLength, Color.cyan);
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
        if (lastGroundObject != null)
        {
            RestoreMaterial(lastGroundObject);
            lastGroundObject = null;
        }

        isGroundedGizmo = false;
        lastGroundID = -999;
        lastGroundPosition = Vector3.zero;
        lastRotation = Quaternion.identity;
    }

    private void RestoreMaterial(GameObject obj)
    {
        if (obj == null) return;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && originalMaterials.ContainsKey(obj))
        {
            renderer.material = originalMaterials[obj];
            originalMaterials.Remove(obj);
        }
    }

    private void GetObjectSize()
    {
        if (playerCollider != null)
        {
            width = playerCollider.bounds.size.x;
            gizmoDirection = -transform.up;
            gizmoRadius = width / 4;
            gizmoLength = (playerCollider.bounds.size.y - gizmoRadius) * 1.11f;
        }
        else
        {
            Debug.LogError("El GameObject no tiene un CapsuleCollider.");
        }
    }
}
