using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = GameManager.Instance.GetInputManager();
        inputManager.SetMenuActionMap();
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
        Application.Quit();
        Debug.Log("Game is quitting...");
    }
}

