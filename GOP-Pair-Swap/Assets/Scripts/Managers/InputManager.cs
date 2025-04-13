using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Vector2 MoveInput { get; private set; }
    public Vector2 CameraInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool CrouchPressed { get; private set; }
    public bool CrouchReleased { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool DashInput { get; private set; }
    public bool RestartInput { get; private set; }
    public bool PauseInput { get; private set; }
    public bool ShootInput { get; private set; }
    public bool BackInput { get; private set; }

    // Reference to player input component.
    private PlayerInput playerInput;

    // Player input actions set in the controls input action.
    private InputAction moveAction;
    private InputAction cameraAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction dashAction;
    [HideInInspector] public InputAction jumpAction;
    private InputAction restartAction;
    [HideInInspector] public InputAction pauseAction;
    private InputAction shootAction;
    private InputAction backAction;

    [Header("Rebinds Loading")]
    public InputActionAsset actions;

    public InputActionMap inGame;
    public InputActionMap uI;

    void Awake()
    {
        // Store the rebinded controls player prefs key in a variables and load that if it exists.
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);

        if (instance == null)
        {
            instance = this;
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
        cameraAction = playerInput.actions["Camera"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
        dashAction = playerInput.actions["Dash"];
        jumpAction = playerInput.actions["Jump"];
        restartAction = playerInput.actions["Restart"];
        pauseAction = playerInput.actions["Pause"];
        shootAction = playerInput.actions["Shoot"];
        backAction = playerInput.actions["BackButton"];
    }

    void UpdateInputs()
    {
        string controlScheme = playerInput.currentControlScheme;

        // Update the input actions based on whether or not an input is used.
        MoveInput = moveAction.ReadValue<Vector2>();
        CameraInput = cameraAction.ReadValue<Vector2>();
        SprintInput = sprintAction.IsPressed();
        CrouchPressed = crouchAction.WasPressedThisFrame();
        CrouchReleased = crouchAction.WasReleasedThisFrame();
        CrouchHeld = crouchAction.IsPressed();
        DashInput = dashAction.WasPressedThisFrame();
        JumpInput = jumpAction.WasPressedThisFrame();
        RestartInput = restartAction.WasPressedThisFrame();
        PauseInput = pauseAction.WasPressedThisFrame();
        ShootInput = shootAction.WasPressedThisFrame();
        BackInput = backAction.WasPressedThisFrame();
    }
}