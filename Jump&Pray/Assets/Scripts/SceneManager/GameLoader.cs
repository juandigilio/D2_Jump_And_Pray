using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private CustomScene gameLoader;
    [SerializeField] private CustomScene mainScene;
    [SerializeField] private CustomScene mainMenu;
    [SerializeField] private CustomScene optionsScene;
    [SerializeField] private List<CustomScene> scenesPool;
    [SerializeField] private CustomScene winingScene;


    void Start()
    {
        SceneManager.SetScenes(gameLoader, mainScene, mainMenu, optionsScene, scenesPool, winingScene);

        _ = SceneManager.LoadGame();
    }
}
