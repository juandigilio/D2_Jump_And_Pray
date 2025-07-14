using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform menuStartPosition;
    [SerializeField] private Transform menuRestartPosition;
    [SerializeField] private CinematicFall cinematicFall;
    [SerializeField] private string levelID;

    void Start()
    {
        if (GameManager.Instance.GetPlayerController().IsFirstTime())
        {
            GameManager.Instance.GetPlayerController().SetFirstTime(false);
            EventManager.Instance.TriggerMenuLoaded(menuStartPosition.position);
            Debug.Log("Menu loaded at start position: " + menuStartPosition.position);
        }
        else
        {
            EventManager.Instance.TriggerMenuLoaded(menuRestartPosition.position);
            cinematicFall.StartCinematicFall();
            Debug.Log("Menu loaded at restart position: " + menuRestartPosition.position);
        }

        GameManager.Instance.GetAudioManager().PlayLevelMusic(levelID);
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

