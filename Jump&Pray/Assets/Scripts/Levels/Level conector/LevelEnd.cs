using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private LevelConection levelConection;


    private bool nextLevelLoaded = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!nextLevelLoaded)
        {
            if (other.CompareTag("Player"))
            {
                levelConection.ChangeLevel();
                nextLevelLoaded = true;
            }
        }       
    }
}
