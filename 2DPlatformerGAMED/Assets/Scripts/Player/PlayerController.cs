using System.Collections;
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
    
    
    [Header("Dash Parameters")]
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _dashButtonHeld;
    [SerializeField] private float _dashForce;
    private Vector2[] _directions = new Vector2[]
    {
        new Vector2(1, 0),   // Right
        new Vector2(1, 1).normalized,   // Up-Right
        new Vector2(0, 1),   // Up
        new Vector2(-1, 1).normalized,  // Up-Left
        new Vector2(-1, 0),  // Left
        new Vector2(-1, -1).normalized, // Down-Left
        new Vector2(0, -1),  // Down
        new Vector2(1, -1).normalized   // Down-Right
    };

    [Header("Energy")] 
    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _currentEnergy;


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
        if (_isDashing == false)
        {
            HandleMovement();
        }
        Friction();

        if (_inputHandler.JumpInput)
        {
            _lastJumpTime = _jumpBufferTime;
            if (_lastGroundedTime > 0 && _lastJumpTime > 0 && !_isJumping)
            {
                HandleJump();
            }
        }

        if (_inputHandler.DashInput)
        {
            if (_isGrounded == false && _currentEnergy > 0 && _dashButtonHeld == false && _isDashing == false)
            {
                print($"Raw input: {_inputHandler.MoveInput} vs dash direction: {DashDirection()}");
                StartCoroutine(HandleDash(DashDirection()));
            }
            _dashButtonHeld = true;
        }

        if (_inputHandler.DashInput == false)
        {
            _dashButtonHeld = false;
        }

        if (_inputHandler.JumpInput == false && _isDashing == false)
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
            if (_isDashing)
            {
                _rigidbody2D.gravityScale = 0;
            }
            else
            {
                _rigidbody2D.gravityScale = _gravityScale * _fallGravityMultiplier;
            }
        }
        else
        {
            _rigidbody2D.gravityScale = _gravityScale;
        }

    }

    private Vector2 DashDirection()
    {
        Vector2 input = _inputHandler.MoveInput.normalized;
        if (input == Vector2.zero) return Vector2.zero;

        Vector2 bestDirection = _directions[2];
        float bestAngle = Vector2.Angle(input, bestDirection);

        foreach (var direction in _directions)
        {
            float angle = Vector2.Angle(input, direction);
            if (angle < bestAngle)
            {
                bestAngle = angle;
                bestDirection = direction;
            }
        }
        return bestDirection;
    }

    private void HandleMovement()
    {
        Vector2 input = _inputHandler.MoveInput;

        if (input.sqrMagnitude > 1)
        {
            input = input.normalized;
        }

        input.x = Mathf.RoundToInt(input.x);

        Vector2 targetVelocity = input * _maxMoveSpeed;
        Vector2 speedDifference = targetVelocity - _rigidbody2D.linearVelocity;

        float accelerationRate = (targetVelocity.sqrMagnitude > 0.01f) ? _acceleration : _decceleration;

        float movement =
            Mathf.Pow(Mathf.Abs(speedDifference.x) * accelerationRate, _velPower) * Mathf.Sign(speedDifference.x);

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

    private IEnumerator HandleDash(Vector2 dashDirection)
    {
        _isDashing = true;
        _rigidbody2D.linearVelocityY = 0;
        _rigidbody2D.AddForce(dashDirection * _dashForce, ForceMode2D.Impulse);
        _currentEnergy--;
        yield return new WaitForSeconds(_dashTime);
        _isDashing = false;
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

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
        {
            _currentEnergy = _maxEnergy;
            _lastGroundedTime = _jumpCoyoteTime;
            _isGrounded = true;
            _isJumping = false;
        }
        else
        {
            _lastGroundedTime -= Time.fixedDeltaTime;
            _isGrounded = false;
        }
    }
}
