using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    private void Start()
    {
        TypewriterEffect typewriterEffect = Text.GetComponent<TypewriterEffect>();
        typewriterEffect.StartTyping();
        typewriterEffect.OnTypingFinished += FinishTyping;
    }

    private void FinishTyping()
    {
        StartCoroutine(SlideIn(Player1));
        StartCoroutine(SlideIn(Player2));
    }

    private IEnumerator SlideIn(GameObject obj)
    {
        float time = 0;
        Vector3 startPos = obj.transform.position;
        Vector3 endPos = new Vector3(500, obj.transform.position.y, obj.transform.position.z);
        while (time < 1)
        {
            time += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(startPos, endPos, time);
            yield return null;
        }
    }

    public void StartGame(int playerType)
    {
        PlayerPrefs.SetInt("PlayerType", playerType);

        var hasPlayedBefore = PlayerPrefs.GetInt("HasPlayedBefore");
        if (hasPlayedBefore == 0)
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
        }
        else
        {
            SceneManager.LoadScene(2);

        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }
}
