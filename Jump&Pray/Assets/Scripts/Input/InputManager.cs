using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController characterController;
    [SerializeField] private Cameraman cameraman;

    [SerializeField] private float padSensitivity = 1.0f;
    [SerializeField] private float mouseSensitivity = 1.0f;
    [SerializeField] private float firstPersonMultiplier = 1.0f;
    [SerializeField] private float thirdPersonSensitivity = 1.0f;

    [SerializeField] private string rotateCameraAction = "MoveCamera";
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string jumpAction = "Jump";

    [SerializeField] private string inGameActionMap = "InGame";
    [SerializeField] private string menuActionMap = "Menu";

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject.");
        }
    }

    private void Start()
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
            //playerInput.currentActionMap.FindAction(jumpAction).performed += Jump;
            playerInput.currentActionMap.FindAction(jumpAction).canceled += Jump;
        }
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
            characterController.SetDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.performed)
        {
            characterController.SetDirection(callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.canceled)
        {
            characterController.SetDirection(Vector2.zero);
        }

    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            characterController.LoadJumpCharge();
        }
        if (callbackContext.canceled)
        {
            characterController.ReleaseJumpCharge();
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
}
