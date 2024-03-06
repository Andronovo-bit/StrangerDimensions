using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rbody;
    private Player _player;
    private GameManager _gameManager;
    private Animator _animator;

    private SpriteRenderer _spriteRenderer;
    private bool _isPortalTriggered = false;
    private bool _isGrounded;
    private Dictionary<PlayerType, short> _maxJump = new();

    [SerializeField]
    private float _speed = 5f;
    public float JumpForce = 5f;
    public float GroundCheckRadius = 0.85f;
    public LayerMask GroundLayer;
    public float DashDistance = 5f;

    private Vector2 _moveDirection;
    private enum MovementState
    {
        Idle = 0,
        Running = 1,
        Jumping = 2,
        Falling = 3,
    }

    private MovementState _movementState = MovementState.Idle;
    private float _moveX;
    private float _moveY;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _maxJump.Add(PlayerType.PlayerTop, 1);
        _maxJump.Add(PlayerType.PlayerBottom, 0);

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");

        switch (_movementState)
        {
            case MovementState.Idle:
            case MovementState.Running:
                HandleMovementInput();

                _isGrounded = Physics2D.OverlapCircle(_player.gameObject.transform.position, GroundCheckRadius, GroundLayer);

                if (_isGrounded)
                {
                    HandleJumpInput();
                }
                break;
            case MovementState.Jumping:
                HandleJumpInput();
                break;
            case MovementState.Falling:
                if (_isGrounded)
                {
                    _movementState = MovementState.Idle;
                }
                break;
        }

        HandleDashInput();
    }

    private void HandleMovementInput()
    {

        _moveDirection = new Vector2(_moveX, _moveY).normalized;

        _movementState = _moveX == 0 ? MovementState.Idle : MovementState.Running;
        _animator.SetInteger("state", (int)_movementState);

        if (_moveX > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_moveX < 0)
        {
            _spriteRenderer.flipX = true;
        }

        _rbody.velocity = new Vector2(_moveX * _speed, _rbody.velocity.y);
    }

    private void HandleJumpInput()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (_isGrounded || _player.JumpCount < _maxJump[_player.PlayerType]))
        {
            Jump();
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.PlayerType == PlayerType.PlayerBottom)
        {
            Dash();
        }
    }

    private void Dash()
    {
        Vector2 targetPosition = _rbody.position + _moveDirection * DashDistance;
        StartCoroutine(SmoothDash(_rbody.position, targetPosition, 0.2f));
    }

    private IEnumerator SmoothDash(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            _rbody.position = Vector2.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _rbody.position = end;
    }

    private void Jump()
    {
        _rbody.velocity = new Vector2(_rbody.velocity.x, JumpForce * (int)_player.JumpWay);
        _player.JumpCount++;

        _movementState = MovementState.Jumping;
        _animator.SetInteger("state", (int)_movementState);
        //after 1 second, set isJumping to false
        if (_player.JumpCount > 1)
        {

            //after 1 second, set isJumping to false
            Invoke("SetIsJumpingToFalse", 1f);
        }
        else
        {

            //after 1 second, set isJumping to false
            Invoke("SetIsJumpingToFalse", 0.5f);
        }



        // Optional: Play jump sound effect

        // Optional: Play jump animation

        // Optional: Add a cooldown for jumping 


    }
    private void SetIsJumpingToFalse()
    {
        _movementState = MovementState.Falling;
        _animator.SetInteger("state", (int)_movementState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal") && !_isPortalTriggered)
        {
            Color color = new Color32(0, 255, 34, 14);
            collision.gameObject.GetComponent<SpriteRenderer>().color = color;
            _isPortalTriggered = true;
        }
    }
}