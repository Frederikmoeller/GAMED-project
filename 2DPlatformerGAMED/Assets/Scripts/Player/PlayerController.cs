using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float _acceleration;
    [SerializeField] private float _decceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _velPower;

    [SerializeField] private float _frictionAmount;

    [Header("Jump Parameters")] 
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpCoyoteTime;
    [SerializeField] private float _jumpBufferTime;
    [SerializeField, Range(0.0f, 1)] private float _jumpCutMultiplier = 0.5f;
    [SerializeField] private float _fallGravityMultiplier;
    private float _gravityScale;
    private bool _jumpInputReleased;
    private bool _isJumping;
    
    private float _lastGroundedTime;
    private float _lastJumpTime;

    private Rigidbody2D _rigidbody2D;

    private PlayerInputHandler _inputHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = PlayerInputHandler.Instance;
        _gravityScale = _rigidbody2D.gravityScale;
        _jumpForce = Mathf.Sqrt(_jumpHeight * Physics2D.gravity.y * _gravityScale * -2) * _rigidbody2D.mass;
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        HandleMovement();
        Friction();

        if (_inputHandler.JumpInput)
        {
            _lastJumpTime = _jumpBufferTime;
            if (_lastGroundedTime > 0 && _lastJumpTime > 0 && !_isJumping)
            {
                HandleJump();
            }
        }

        if (_inputHandler.JumpInput == false)
        {
            if (_rigidbody2D.linearVelocityY > 0 && _isJumping)
            {
                _rigidbody2D.AddForce(Vector2.down * _rigidbody2D.linearVelocityY * (1 - _jumpCutMultiplier), ForceMode2D.Impulse);
            }
            _jumpInputReleased = true;
            _lastJumpTime = 0;
        }

        if (_rigidbody2D.linearVelocityY < 0)
        {
            _rigidbody2D.gravityScale = _gravityScale * _fallGravityMultiplier;
        }
        else
        {
            _rigidbody2D.gravityScale = _gravityScale;
        }

    }

    private void HandleMovement()
    {
        float targetSpeed = _inputHandler.MoveInput.x * _maxMoveSpeed;

        float speedDifference = targetSpeed - _rigidbody2D.linearVelocityX;

        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _velPower) * Mathf.Sign(speedDifference);
        
        _rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void HandleJump()
    {
        _rigidbody2D.linearVelocityY = 0;
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _lastGroundedTime = 0;
        _lastJumpTime = 0; 
        _isJumping = true;
        _jumpInputReleased = false;
    }

    private void Friction()
    {
        if (_lastGroundedTime > 0 && Mathf.Abs(_inputHandler.MoveInput.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(_rigidbody2D.linearVelocityX), Mathf.Abs(_frictionAmount));

            amount *= Mathf.Sign(_rigidbody2D.linearVelocityX);
            
            _rigidbody2D.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private bool GroundCheck()
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
        {
            _lastGroundedTime = _jumpCoyoteTime;
            _isGrounded = true;
            _isJumping = false;
            return true;
        }
        else
        {
            _lastGroundedTime -= Time.fixedDeltaTime;
            _isGrounded = false;
            return false;
        }
    }
}
