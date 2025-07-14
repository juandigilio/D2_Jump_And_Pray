using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private ArcadeBehaviour arcadeBehaviour;

    [Header("UI Components")]
    [SerializeField] private Slider mouseSlider;
    [SerializeField] private Slider gamepadSlider;
    [SerializeField] private Button quitButton;
    //[SerializeField] private GameObject firstSelectedUIElement;

    private bool isPaused;

    private void Start()
    {
        GameManager.Instance.RegisterOptionsManager(this);

        isPaused = false;

        mouseSlider.onValueChanged.AddListener(UpdateMouseSensitivity);
        gamepadSlider.onValueChanged.AddListener(UpdateGamepadSensitivity);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowOptions()
    {
        if (!arcadeBehaviour.IsDropping())
        {       
            if (!isPaused)
            {
                UpdateSlidersValues();
                EventManager.Instance.TriggerShowOptionsMenu();
                EventSystem.current.SetSelectedGameObject(mouseSlider.gameObject);
            }
            else
            {
                EventManager.Instance.TriggerHideOptionsMenu();
                EventSystem.current.SetSelectedGameObject(null);
            }
            isPaused = !isPaused;
        }
    }

    private void UpdateMouseSensitivity(float value)
    {
        PlayerConfig.mouseSensitivity = value;
    }

    private void UpdateGamepadSensitivity(float value)
    {
        PlayerConfig.gamepadSensitivity = value;
    }

    private void UpdateSlidersValues()
    {
        mouseSlider.value = PlayerConfig.mouseSensitivity;
        gamepadSlider.value = PlayerConfig.gamepadSensitivity;
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
