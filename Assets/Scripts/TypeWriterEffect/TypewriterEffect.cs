using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private float delayBeforeStart = 1f;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private string leadingChar = "|";
    [SerializeField] private bool leadingCharBeforeDelay = false;

    public Action OnTypingFinished;

    private string _writer;
    private Coroutine _typingCoroutine;

    public void StartTyping()
    {
        if (textObject == null)
        {
            textObject = GetComponent<TextMeshProUGUI>();
        }

        _writer = textObject.text;
        textObject.text = leadingCharBeforeDelay ? leadingChar : "";

        _typingCoroutine = StartCoroutine(nameof(TMPTypeWriter));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _typingCoroutine != null)
        {
            StartCoroutine(PressSpaceToSkip());
        }
    }

    private IEnumerator PressSpaceToSkip()
    {
        StopCoroutine(_typingCoroutine);
        textObject.text = _writer;
        yield return new WaitForSeconds(1f);
        OnTypingFinished?.Invoke();
        _typingCoroutine = null;

    }

    private IEnumerator TMPTypeWriter()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (var c in _writer)
        {
            RemoveLeadingChar();
            AddCharAndLeadingChar(c);
            PlayRandomSound();
            yield return new WaitForSeconds(timeBtwChars);
        }

        RemoveLeadingChar();
        yield return new WaitForSeconds(0.5f);
        OnTypingFinished?.Invoke();
    }

    private void RemoveLeadingChar()
    {
        if (textObject.text.Length > 0)
            textObject.text = textObject.text[..^leadingChar.Length];
    }

    private void AddCharAndLeadingChar(char c)
    {
        textObject.text += c;
        textObject.text += leadingChar;
    }

    private void PlayRandomSound()
    {
        RandomSound.Singleton.SetSourceClip(RandomSound.Singleton.audioClips[Random.Range(0, RandomSound.Singleton.audioClips.Length)]);
    }
}