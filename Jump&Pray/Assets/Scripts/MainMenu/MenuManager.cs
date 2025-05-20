using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = GameManager.Instance.GetInputManager();
    }


    void Update()
    {
        
    }

    public void LoadGame()
    {
        SceneManager.LoadTutorialScene();
    }

    public void LoadOptions()
    {
        //GameManager.Instance.LoadOptionsMenu();
    }

    public void Unload()
    {
        SceneManager.UnloadMainMenuScene();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        Debug.Log("Game is quitting...");
    }
}

