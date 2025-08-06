using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 2f;
    [SerializeField] private float fallDelay = 0.2f;
    [SerializeField] private float resetTime = 3f;
    [SerializeField] private float shakeAmount = 0.05f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;
    private bool isTriggered = false;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        col.isTrigger = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateFall();
        }
    }

    public void ActivateFall()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(ShakeAndFall());
    }

    IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;
        Vector3 original = transform.position;

        while (elapsed < shakeDuration)
        {
            transform.position = original + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(resetTime);

        rb.isKinematic = true;
        rb.useGravity = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isTriggered = false;
    }
}
