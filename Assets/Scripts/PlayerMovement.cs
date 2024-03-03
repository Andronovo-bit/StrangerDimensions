using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rbody;
    private Player _player;
    private GameManager _gameManager;
    private bool _isPortalTriggered = false;
    private bool _isGrounded;
    private Dictionary<PlayerType, short> _maxJump = new();

    [SerializeField]
    private float _speed = 5f;
    public float JumpForce = 5f;
    public float GroundCheckRadius = 0.2f;
    public LayerMask GroundLayer;
    public float DashDistance = 5f;

    private Vector2 _moveDirection;
    private bool _isDashing = false;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _maxJump.Add(PlayerType.PlayerTop, 2);
        _maxJump.Add(PlayerType.PlayerBottom, 1);
    }

    private void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleDashInput();
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;

        _rbody.velocity = new Vector2(moveX * _speed, _rbody.velocity.y);
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