using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private MovementBehaviour movementBehaviour;

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
            playerInput.currentActionMap.FindAction("Jump").started += jumpBehaviour.Jump;
            playerInput.currentActionMap.FindAction("Jump").performed += jumpBehaviour.Jump;
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
            movementBehaviour.SetInputDirection(callbackContext.ReadValue<Vector2>());
            print("Input started");
        }

        if (callbackContext.performed)
        {
            movementBehaviour.SetInputDirection(callbackContext.ReadValue<Vector2>());
            print("Input performed");
        }

        if (callbackContext.canceled)
        {
            movementBehaviour.SetInputDirection(Vector2.zero);
            print("Input canceled");
        }

    }
}
