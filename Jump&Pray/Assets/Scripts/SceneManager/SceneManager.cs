using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneManager
{
    private static HashSet<string> loadedScenes = new HashSet<string>();
    private static CustomScene mainScene;
    private static CustomScene mainMenu;
    private static List<CustomScene> scenesPool = new List<CustomScene>();
    private static CustomScene winingScene;

    private static int index = 0;


    private static async Task LoadSceneAsync(CustomScene scene)
    {
        if (!IsSceneLoaded(scene))
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            loadedScenes.Add(scene.sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + scene + " is already loaded.");
        }
    }

    private static async Task UnloadSceneAsync(CustomScene scene)
    {
        if (IsSceneLoaded(scene))
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.sceneName);

            while (!asyncUnload.isDone)
            {
                await Task.Yield();
            }

            loadedScenes.Remove(scene.sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + scene + " is not loaded.");
        }
    }

    private static bool IsSceneLoaded(CustomScene scene)
    {
        return loadedScenes.Contains(scene.sceneName);
    }

    private static async Task LoadMainScene()
    {
        await LoadSceneAsync(mainScene);
    }

    private static void UnloadAll()
    {
        foreach (CustomScene scene in scenesPool)
        {
            _ = UnloadSceneAsync(scene);
        }

        _ = UnloadSceneAsync(winingScene);
    }

    public static void LoadNextSceneAsync()
    {
        if ((index + 1) < scenesPool.Count)
        {
            index++;

            _ = LoadSceneAsync(scenesPool[index]);
        }
    }

    public static void UnloadLastScene()
    {
        if (index > 0)
        {
            _ = UnloadSceneAsync(scenesPool[index - 1]);
        }
    }

    public static void SetScenes(CustomScene main, CustomScene menu, List<CustomScene> sceneDictionary, CustomScene win)
    {
        mainScene = main;
        mainMenu = menu;

        foreach (CustomScene scene in sceneDictionary)
        {
            scenesPool.Add(scene);
        }

        winingScene = win;
    }

    public static async Task LoadGame()
    {
        await LoadMainScene();
        LoadMenuScene();
    }

    public static void LoadMenuScene()
    {
        UnloadAll();

        _ = LoadSceneAsync(mainMenu);
    }

    public static void LoadWiningScene()
    {
        _ = LoadSceneAsync(winingScene);
    }
}
