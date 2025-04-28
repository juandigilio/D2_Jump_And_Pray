using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

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
            playerInput.SwitchCurrentActionMap("InGame");

            playerInput.currentActionMap.FindAction("Move").started += Move;
            playerInput.currentActionMap.FindAction("Move").performed += Move;
            playerInput.currentActionMap.FindAction("Move").canceled += Move;

            playerInput.currentActionMap.FindAction("Jump").started += Jump;
            playerInput.currentActionMap.FindAction("Jump").performed += Jump;
            playerInput.currentActionMap.FindAction("Jump").canceled += Jump;
        }
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {

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

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            characterController.LoadJumpCharge ();
        }

        if (callbackContext.performed)
        {
            //characterController.Jump();
        }

        if (callbackContext.canceled)
        {
            characterController.ReleaseJumpCharge();
        }
    }
}
