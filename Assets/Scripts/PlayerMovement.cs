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
    private bool _isDashing = false;
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

        HandleMovementInput();
        HandleJumpInput();
        HandleDashInput();
    }

    private void HandleMovementInput()
    {

        _moveDirection = new Vector2(_moveX, _moveY).normalized;

        // Cache references to components to avoid calling GetComponent multiple times
        // Animator playerAnimator = _player.GetComponent<Animator>();
        // SpriteRenderer playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();

        bool isRunning = _moveX != 0;
        _animator.SetBool("isRunning", isRunning);

        if (isRunning)
        {
            _spriteRenderer.flipX = _moveX < 0;
        }

        _rbody.velocity = new Vector2(_moveX * _speed, _rbody.velocity.y);
    }

    private void HandleJumpInput()
    {
        _isGrounded = Physics2D.OverlapCircle(_player.gameObject.transform.position, GroundCheckRadius, GroundLayer);

        if (_isGrounded)
        {
            _player.JumpCount = 0;
        }

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
        _rbody.MovePosition(_rbody.position + _moveDirection * DashDistance);
    }

    private void Jump()
    {
        _rbody.velocity = new Vector2(_rbody.velocity.x, JumpForce * (int)_player.JumpWay);
        _player.JumpCount++;

        _animator.SetBool("isJumping", true);

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
        _animator.SetBool("isJumping", false);
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