using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] Player player;
    private AudioSource _finishSound;

    private bool _isLevelComplete = false;
    private void Start()
    {
        _finishSound = GetComponent<AudioSource>();
        Debug.Log("Finish line is ready");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_isLevelComplete)
        {
            _finishSound.Play();
            Debug.Log("Player reached the finish line");
            _isLevelComplete = true;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Invoke(nameof(CompleteLevel), 2f);
        }
    }

    private void CompleteLevel()
    {
        _isLevelComplete = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Level is complete");
    }
}
