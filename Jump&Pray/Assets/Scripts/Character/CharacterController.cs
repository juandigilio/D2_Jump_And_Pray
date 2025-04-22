using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private JumpBehaviour jumpBehaviour;

    Vector2 inputDirection;

    private void Start()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
        if (movementBehaviour == null)
        {
            Debug.LogError("MovementBehaviour not found on the GameObject.");
        }
        jumpBehaviour = GetComponent<JumpBehaviour>();
        if (jumpBehaviour == null)
        {
            Debug.LogError("JumpBehaviour not found on the GameObject.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void Jump()
    {
        jumpBehaviour.Jump();
    }

    public void SetDirection(Vector2 input)
    {
        inputDirection = input;
        movementBehaviour.SetInputDirection(inputDirection);
    }
}
