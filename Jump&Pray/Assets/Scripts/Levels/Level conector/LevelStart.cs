using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.UnloadLastScene();
        }
    }
}
