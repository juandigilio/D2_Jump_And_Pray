using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Cameraman cameraman;
    private PlayerController playerController;
    private InputManager inputManager;
    private PlayerInput playerInput;
    private StateManager stateManager;
    private OptionsManager optionsManager;
    private CheatsManager cheatsManager;
    private AudioManager audioManager;

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

    public void RegisterPlayerInput(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
    }

    public void RegisterOptionsManager(OptionsManager optionsManager)
    {
        this.optionsManager = optionsManager;
    }

    public void RegisterCheatsManager(CheatsManager cheatsManager)
    {
        this.cheatsManager = cheatsManager;
    }

    public void RegisterAudioManager(AudioManager audioManager)
    {
        this.audioManager = audioManager;
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

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    public OptionsManager GetOptionsManager()
    {
        return optionsManager;
    }

    public CheatsManager GetCheatsManager()
    {
        return cheatsManager;
    }

    public AudioManager GetAudioManager()
    {
        return audioManager;
    }
}
