using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Vector2 MoveInput { get; private set; }
    public bool PauseInput { get; private set; }

    // Reference to player input component.
    [SerializeField] private PlayerInput playerInput;

    // Player input actions set in the controls input action.
    private InputAction moveAction;
    private InputAction pauseAction;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of InputManager exists
        // Set Instance to this if it's null, otherwise destroy this object
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set reference
        playerInput = GetComponent<PlayerInput>();

        // Set the input actions equal to input actions in the controls input action.
        SetupInputActions();
    }

    void Update()
    {
        UpdateInputs();
    }

    void SetupInputActions()
    {
        // Set each input action variable to the input action stored in the controls input action.
        moveAction = playerInput.actions["Move"];
        pauseAction = playerInput.actions["Pause"];
    }

    void UpdateInputs()
    {
        // Update the input actions based on whether or not an input is used.
        MoveInput = moveAction.ReadValue<Vector2>();
        PauseInput = pauseAction.WasPressedThisFrame();
    }
}