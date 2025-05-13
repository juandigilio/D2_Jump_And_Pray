using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private JumpBehaviour jumpBehaviour;

    Vector2 inputDirection;

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

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

    public void LoadJumpCharge()
    {
        jumpBehaviour.StartCharge();
    }

    public void ReleaseJumpCharge()
    {
        jumpBehaviour.StopCharge();
    }

    public void SetDirection(Vector2 input)
    {
        inputDirection = input;
        movementBehaviour.SetInputDirection(inputDirection);
    }
}
