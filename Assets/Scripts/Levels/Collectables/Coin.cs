using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    private Vector3 rotationAxis = new Vector3(0, 0, 1);
    private float rotationSpeed = 100f;


    private void FixedUpdate()
    {
        RotateCoin();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        gameObject.SetActive(false);
    }

    private void RotateCoin()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
