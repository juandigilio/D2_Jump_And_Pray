using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Cameraman cameraman;
    private PlayerController playerController;
    private InputManager inputManager;
    private StateManager stateManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterCameraman(Cameraman cameraman)
    {
        this.cameraman = cameraman;
    }

    public void RegisterPlayer(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void RegisterInputManager(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public void RegisterStateManager(StateManager stateManager)
    {
        this.stateManager = stateManager;
    }

    public Cameraman GetCameraman()
    {
        return cameraman;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public InputManager GetInputManager()
    {
        return inputManager;
    }

    public StateManager GetStateManager()
    {
        return stateManager;
    }
}
