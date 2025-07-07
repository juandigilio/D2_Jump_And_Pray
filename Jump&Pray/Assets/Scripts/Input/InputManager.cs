using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float padSensitivity = 1.0f;
    [SerializeField] private float mouseSensitivity = 1.0f;
    [SerializeField] private float firstPersonMultiplier = 1.0f;
    [SerializeField] private float thirdPersonSensitivity = 1.0f;

    [SerializeField] private string rotateCameraAction = "MoveCamera";
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string jumpAction = "Jump";
    [SerializeField] private string rollAction = "Roll";

    [SerializeField] private string pauseAction = "Pause";
    //[SerializeField] private string quitAction = "Quit";

    [SerializeField] private string nextLevelAction = "NextLevel";
    //[SerializeField] private string godModeAction = "GodMode";
    //[SerializeField] private string superSpeedAction = "SuperSpeed";

    [SerializeField] private string inGameActionMap = "InGame";
    [SerializeField] private string menuActionMap = "Menu";

    private PlayerInput playerInput;
    private PlayerController playerController;
    private Cameraman cameraman;
    private OptionsManager optionsManager;
    private CheatsManager cheatsManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject.");
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.RegisterInputManager(this);
        GameManager.Instance.RegisterPlayerInput(playerInput);
    }

    private void Start()
    {
        cheatsManager = GameManager.Instance.GetCheatsManager();

        if (cheatsManager == null)
        {
            Debug.LogError("CheatsManager not found in GameManager.");
        }


        LoadActions();

        cameraman = GameManager.Instance.GetCameraman();
        playerController = GameManager.Instance.GetPlayerController();
        optionsManager = GameManager.Instance.GetOptionsManager();
    }

    private void Update()
    {
        SetMouseCameraRotation();
    }

    private void FixedUpdate()
    {
        CheckPlayerConfig();
    }

    private void SetGamepadCameraRotation(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started || callbackContext.performed)
        {
            cameraman.UpdateInputRotation(callbackContext.ReadValue<Vector2>() * PlayerConfig.gamepadSensitivity);
        }
        if (callbackContext.canceled)
        {
            cameraman.UpdateInputRotation(Vector2.zero);
        }
    }

    private void SetMouseCameraRotation()
    {
        cameraman.UpdateInputRotation(Mouse.current.delta.ReadValue() * PlayerConfig.mouseSensitivity);
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.SetDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.performed)
        {
            playerController.SetDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.canceled)
        {
            playerController.SetDirection(Vector2.zero);
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.LoadJumpCharge();
        }
        if (callbackContext.canceled)
        {
            playerController.ReleaseJumpCharge();
        }
    }

    private void Roll(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            playerController.Roll();
        }
    }

    private void CheckPlayerConfig()
    {
        if (PlayerConfig.mouseSensitivity != mouseSensitivity)
        {
            PlayerConfig.mouseSensitivity = mouseSensitivity;
        }
        if (PlayerConfig.gamepadSensitivity != padSensitivity)
        {
            PlayerConfig.gamepadSensitivity = padSensitivity;
        }
        if (PlayerConfig.firstPersonMultiplier != firstPersonMultiplier)
        {
            PlayerConfig.firstPersonMultiplier = firstPersonMultiplier;
        }
        if (PlayerConfig.thirdPersonMultiplier != thirdPersonSensitivity)
        {
            PlayerConfig.thirdPersonMultiplier = thirdPersonSensitivity;
        }
    }

    public void SetInGameActionMap()
    {
        if (playerInput.inputIsActive)
        {
            playerInput.SwitchCurrentActionMap(inGameActionMap);
        }
    }

    public void SetMenuActionMap()
    {
        playerInput.SwitchCurrentActionMap(menuActionMap);
    }

    public void SetActionMap(ActionMapType type)
    {
        switch (type)
        {
            case ActionMapType.Options:
                {
                    SetMenuActionMap();
                    break;
                }
            case ActionMapType.InGame:
                {
                    SetInGameActionMap();
                    break;
                }
            default:
                {
                    SetInGameActionMap();
                    break;
                }
        }
    }

    private void ShowOptions(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            optionsManager.ShowOptions();
        }
    }

    private void GoToNextLevel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            cheatsManager.GoToNextLevel();
        }       
    }

    private void LoadActions()
    {
        playerInput.ActivateInput();

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(inGameActionMap);

            playerInput.currentActionMap.FindAction(rotateCameraAction).started += SetGamepadCameraRotation;
            playerInput.currentActionMap.FindAction(rotateCameraAction).performed += SetGamepadCameraRotation;
            playerInput.currentActionMap.FindAction(rotateCameraAction).canceled += SetGamepadCameraRotation;

            playerInput.currentActionMap.FindAction(moveAction).started += Move;
            playerInput.currentActionMap.FindAction(moveAction).performed += Move;
            playerInput.currentActionMap.FindAction(moveAction).canceled += Move;

            playerInput.currentActionMap.FindAction(jumpAction).started += Jump;
            playerInput.currentActionMap.FindAction(jumpAction).canceled += Jump;

            playerInput.currentActionMap.FindAction(rollAction).started += Roll;

            playerInput.currentActionMap.FindAction(pauseAction).started += ShowOptions;

            playerInput.currentActionMap.FindAction(nextLevelAction).started += GoToNextLevel;

            //playerInput.currentActionMap.FindAction(quitAction).started += GameManager.Instance.QuitGame;

            playerInput.SwitchCurrentActionMap(menuActionMap);

            playerInput.currentActionMap.FindAction(pauseAction).started += ShowOptions;
        }
    }
}
