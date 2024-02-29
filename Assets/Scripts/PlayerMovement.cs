using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D _rbody;
    private Player _player;
    private GameManager _gameManager;
    private bool _isPortalTriggered = false;
    [SerializeField] private short _jumpWay;
    [SerializeField] private float speed = 5f;
    void Awake()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _jumpWay = (short)_player.jumpWay;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
         _rbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, _rbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rbody.velocity = new Vector2(_rbody.velocity.x, speed * _jumpWay);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal" && !_isPortalTriggered)
        {
            
            //create color HSV
            Color color = new Color32(0, 255, 34, 14);
            collision.gameObject.GetComponent<SpriteRenderer>().color = color;
            StartCoroutine(_gameManager.ChangeCameraPosition());
            _isPortalTriggered = true;
        }
    }


}
