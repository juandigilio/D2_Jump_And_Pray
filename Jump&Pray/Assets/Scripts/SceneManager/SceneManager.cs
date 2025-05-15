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


    public static void LoadNextSceneAsync()
    {
        if ((index + 1) < scenesPool.Count)
        {
            index++;

            LoadSceneAsync(scenesPool[index]);
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

    public static async Task LoadSceneAsync(CustomScene scene)
    {
        if (!loadedScenes.Contains(scene.sceneName))
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

    public static async Task UnloadSceneAsync(CustomScene scene)
    {
        if (loadedScenes.Contains(scene.sceneName))
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

    public static bool IsSceneLoaded(CustomScene scene)
    {
        return loadedScenes.Contains(scene.sceneName);
    }

    public static void LoadMainScene()
    {
        LoadSceneAsync(mainScene);
    }

    public static void LoadMenuScene()
    {
        LoadSceneAsync(mainMenu);
    }

    public static void LoadWiningScene()
    {
        LoadSceneAsync(winingScene);
    }
}
