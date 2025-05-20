using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = GameManager.Instance.GetInputManager();
        //inputManager.SetMenuActionMap();
    }


    void Update()
    {
        
    }

    public void LoadGame()
    {
        //GameManager.Instance.LoadNextScene();
    }

    public void LoadOptions()
    {
        //GameManager.Instance.LoadOptionsMenu();
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

