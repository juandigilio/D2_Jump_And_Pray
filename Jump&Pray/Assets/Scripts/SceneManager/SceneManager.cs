using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneManager
{
    private static HashSet<string> loadedScenes = new HashSet<string>();
    private static CustomScene gameLoaderScene;
    private static CustomScene mainScene;
    private static CustomScene mainMenuScene;
    private static CustomScene optionsScene;
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

        SetInputActionMap(scene);
    }

    private static async Task UnloadSceneAsync(CustomScene scene)
    {
        if (IsSceneLoaded(scene))
        {
            Debug.Log("Unloading scene: " + scene.sceneName);
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.sceneName);

            while (!asyncUnload.isDone)
            {
                await Task.Yield();
            }

            loadedScenes.Remove(scene.sceneName);
        }
        else
        {
            //Debug.LogWarning("Scene " + scene + " is not loaded.");
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

    private static async Task UnloadAll()
    {
        await UnloadSceneAsync(gameLoaderScene);
        await UnloadSceneAsync(optionsScene);

        foreach (CustomScene scene in scenesPool)
        {
            await UnloadSceneAsync(scene);
        }
        
        await UnloadSceneAsync(winingScene);
    }

    private static void SetInputActionMap(CustomScene scene)
    {
        InputManager inputManager = GameManager.Instance.GetInputManager();

        inputManager.SetActionMap(scene.actionMapType);
    }

    public static void SetScenes(CustomScene gameLoader, CustomScene main, CustomScene menu, CustomScene options, List<CustomScene> sceneDictionary, CustomScene win)
    {
        gameLoaderScene = gameLoader;
        mainScene = main;
        mainMenuScene = menu;
        optionsScene = options;

        foreach (CustomScene scene in sceneDictionary)
        {
            scenesPool.Add(scene);
        }

        winingScene = win;
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
            Debug.Log("Unloading scene: " + scenesPool[index - 1].sceneName);
            Debug.Log("Index: " + index);
            _ = UnloadSceneAsync(scenesPool[index - 1]);
        }
    }

    public static async Task LoadGame()
    {
        await LoadMainScene();
        LoadMenuScene();

        _ = UnloadSceneAsync(gameLoaderScene);
    }

    public static void LoadMenuScene()
    {
        _ = UnloadAll();

        _ = LoadSceneAsync(mainMenuScene);
    }

    public static void LoadTutorialScene()
    {
        index = 0;

        _ = LoadSceneAsync(scenesPool[index]);
    }

    public static void UnloadMainMenuScene()
    {
        _ = UnloadSceneAsync(mainMenuScene);
    }

    public static void LoadWiningScene()
    {
        _ = LoadSceneAsync(winingScene);
    }

    public static void LoadOptionsScene()
    {
        _ = LoadSceneAsync(optionsScene);
    }

    public static void UnloadOptionsScene()
    {
        _ = UnloadSceneAsync(optionsScene);
    }
}
