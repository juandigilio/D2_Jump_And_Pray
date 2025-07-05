using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform menuStartPosition;


    void Start()
    {
        EventManager.Instance.TriggerMenuLoaded(menuStartPosition.position);
    }

    public void LoadGame()
    {
        SceneManager.LoadTutorialScene();
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

