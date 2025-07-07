using UnityEngine;
using UnityEngine.Rendering;

public class RotatingPlatformRing : MonoBehaviour
{
    public GameObject platformPrefab;
    public Vector3 centerPosition;
    [Range(2, 16)] public int numberOfPlatforms = 4; // Debe ser par
    public float distanceBetweenPlatforms = 3f;
    public float rotationSpeed = 20f;
    public float verticalAmplitude = 0.5f;
    public float verticalSpeed = 2f;

    private GameObject[] platforms;
    private float radius;

    private void Start()
    {
        ValidatePlatformCount();
        CalculateRadius();
        InstantiatePlatforms();
        SetInitialPositions();
    }

    private void Update()
    {
        CalculateRadius();
        UpdatePlatformPositions();
    }

    private void ValidatePlatformCount()
    {
        if (numberOfPlatforms % 2 != 0)
        {
            Debug.LogWarning("El número de plataformas debe ser par");
            numberOfPlatforms += 1;
        }
    }

    private void CalculateRadius()
    {
        float angleBetween = 360f / numberOfPlatforms;
        float angleRad = Mathf.Deg2Rad * angleBetween;

        radius = distanceBetweenPlatforms / (2 * Mathf.Sin(angleRad / 2));
    }

    private void InstantiatePlatforms()
    {
        platforms = new GameObject[numberOfPlatforms];

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            platforms[i] = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity, transform);
        }
    }

    private void SetInitialPositions()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            Vector3 position = CalculatePlatformPosition(i, 0f);
            platforms[i].transform.position = position;
        }
    }

    private void UpdatePlatformPositions()
    {
        float time = Time.time;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            Vector3 position = CalculatePlatformPosition(i, time);
            platforms[i].transform.position = position;
        }
    }

    private Vector3 CalculatePlatformPosition(int index, float time)
    {
        centerPosition = transform.position;
        float anglePerPlatform = 360f / numberOfPlatforms;
        float totalAngle = time * rotationSpeed + anglePerPlatform * index;
        float angleRad = Mathf.Deg2Rad * totalAngle;

        Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * radius;
        float verticalOffset = Mathf.Sin(time * verticalSpeed + index * Mathf.PI) * verticalAmplitude;

        return centerPosition + offset + Vector3.up * verticalOffset;
    }
}
