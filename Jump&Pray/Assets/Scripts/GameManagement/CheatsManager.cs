using UnityEngine;

public class CheatsManager : MonoBehaviour
{
    private PlayerController playerController;
    private string currentSceneName;

    private void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not initialized.");
        }
        else
        {
            GameManager.Instance.RegisterCheatsManager(this);
        }
    }

    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }      
    }

    public void GoToNextLevel()
    {
        if (!SceneManager.IsMainMenuSceneLoaded())
        {
            playerController.AddLife();
            SceneManager.LoadNextSceneAsync();
            SceneManager.UnloadLastScene();
        }
        else
        {
            Debug.LogWarning("Cannot go to next level from the main menu.");
        }
    }

    public void SetCurrentSceneName(string sceneName)
    {
        currentSceneName = sceneName;
    }
}
