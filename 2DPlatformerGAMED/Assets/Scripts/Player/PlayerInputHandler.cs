using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset _playerControls;

    [Header("Action Map Name References")] 
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")] 
    [SerializeField] private string _move = "Move";
    [SerializeField] private string _jump = "Jump";
    [SerializeField] private string _dash = "Dash";
    [SerializeField] private string _pause = "Pause";

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _pauseAction;
    
    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool PauseInput { get; private set; }
    
    public static PlayerInputHandler PlayerInputHandlerInstance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (PlayerInputHandlerInstance == null)
        {
            PlayerInputHandlerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _moveAction = _playerControls.FindActionMap(actionMapName).FindAction(_move);
        _jumpAction = _playerControls.FindActionMap(actionMapName).FindAction(_jump);
        _dashAction = _playerControls.FindActionMap(actionMapName).FindAction(_dash);
        _pauseAction = _playerControls.FindActionMap(actionMapName).FindAction(_pause);
        RegisterInputActions();
    }

    void Update()
    {
        
    }

    private void RegisterInputActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;
        
        _jumpAction.performed += context => JumpInput = true;
        _jumpAction.canceled += context => JumpInput = false;
        
        _dashAction.performed += context => DashInput = true;
        _dashAction.canceled += context => DashInput = false;
        
        _pauseAction.performed += context => PauseInput = true;
        _pauseAction.canceled += context => PauseInput = false;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _dashAction.Enable();
        _pauseAction.Enable();
    }
    
    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _dashAction.Disable();
        _pauseAction.Disable();
    }
}
