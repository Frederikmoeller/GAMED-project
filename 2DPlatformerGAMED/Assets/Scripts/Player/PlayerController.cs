using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")] 
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxMoveSpeed;

    [Header("Jump Parameters")] 
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private bool _isGrounded;

    private Rigidbody2D _rigidbody2D;

    private PlayerInputHandler _inputHandler;
    private Vector2 _currentMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = PlayerInputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 horizontalMovement = new Vector2(_inputHandler.MoveInput.x, _inputHandler.MoveInput.y);
         
        horizontalMovement.Normalize();

        _currentMovement.x = horizontalMovement.x * _acceleration;
        
        HandleJump();

        _rigidbody2D.AddForce(_currentMovement, ForceMode2D.Force);
        if (Mathf.Abs(_rigidbody2D.linearVelocityX) >= _maxMoveSpeed)
        {
            _rigidbody2D.linearVelocityX = _maxMoveSpeed * _inputHandler.MoveInput.x;
        }
    }

    private void HandleJump()
    {
        if (_isGrounded)
        {
            _currentMovement.y = -0.5f;
            if (_inputHandler.JumpInput)
            {
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
