using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
    [SerializeField] private Material globalSkybox;

    private void Start()
    {
        RenderSettings.skybox = globalSkybox;
    }
}
