using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public AudioSource DeathSound;

    private int _lives = 3;
    private GameManager _gameManager;

    private Rigidbody2D _rbody;
    private Animator _animator;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        DeathSound.Play();
        _lives--;
        _animator.SetTrigger("death");
        _rbody.bodyType = RigidbodyType2D.Static;

        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        //get last scene index
        SceneManager.LoadScene(scene.buildIndex);
    }
}
