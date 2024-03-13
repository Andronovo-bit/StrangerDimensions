using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject[] StoryTexts;
    [SerializeField] GameObject[] TutorialTopTexts;
    [SerializeField] GameObject[] TutorialBottomTexts;
    [SerializeField] GameObject[] TutorialCommonTexts;
    [SerializeField] GameObject m_skipText;
    [SerializeField] GameObject m_skipIntroText;
    [SerializeField] GameObject m_startingGameText;
    private short _currentStoryIndex = 0;
    private short _currentTutorialTopIndex = 0;
    private short _currentTutorialBottomIndex = 0;
    private short _currentTutorialCommonIndex = 0;
    private bool _isFinishStory;

    private bool _hasPressA = false;
    private bool _hasPressD = false;
    private bool _hasPressSpace = false;
    private bool _hasPressSpaceTwice = false;
    private bool _hasPressLeftShift = false;
    private bool _hasPressQ = false;
    private bool _hasPressE = false;
    private bool _hasPressX = false;
    private float _lastSpacePressTime = 0f;
    private float _doublePressThreshold = 1f; // The maximum time between presses for a double press

    private bool _isFinishTutorialTop = false;
    private bool _isFinishTutorialBottom = false;
    private bool _isFinishTutorialCommon = false;


    void Awake()
    {
        _isFinishStory = _currentStoryIndex >= StoryTexts.Length;

    }
    void Start()
    {
        StoryTexts = null;
        if (StoryTexts?.Length > 0)
        {
            StoryTexts[_currentStoryIndex].SetActive(true);
            TypewriterEffect typewriterEffect = StoryTexts[_currentStoryIndex].GetComponent<TypewriterEffect>();
            typewriterEffect.StartTyping();
            typewriterEffect.OnTypingFinished += FinishTyping;
        }
        else
        {
            m_skipText.SetActive(false);
            _isFinishStory = true;
            StartCoroutine(CameraFocusPlayerOne());

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _hasPressA = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _hasPressD = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _hasPressSpace = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_hasPressSpace && Time.time - _lastSpacePressTime <= _doublePressThreshold)
            {
                _hasPressSpaceTwice = true;
            }
            else
            {
                _hasPressSpace = true;
            }

            _lastSpacePressTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _hasPressLeftShift = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _hasPressQ = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _hasPressE = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _hasPressX = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_startingGameText.SetActive(true);
            StartCoroutine(StartGameAfterDelay());
        }

    }
    void FixedUpdate()
    {

        if (_isFinishStory && !_isFinishTutorialTop)
        {
            m_skipText.SetActive(false);
            m_skipIntroText.SetActive(true);
            StartCoroutine(CameraFocusPlayerOne());
        }
        if (_isFinishTutorialTop && _isFinishStory && _hasPressSpaceTwice && !_isFinishTutorialBottom)
        {
            StartCoroutine(CameraFocusPlayerTwo());
        }
        if (_isFinishTutorialBottom && _isFinishTutorialTop && _isFinishStory && !_isFinishTutorialCommon)
        {
            StartCoroutine(CameraFocusCommon());
        }

        if (_isFinishTutorialCommon && _isFinishTutorialTop && _isFinishStory && _isFinishTutorialBottom)
        {
            TutorialCommonTexts[2].SetActive(false);
            m_startingGameText.SetActive(true);
            StartCoroutine(StartGameAfterDelay());
        }
    }

    void FinishTyping()
    {
        StoryTexts[_currentStoryIndex].SetActive(false);
        _currentStoryIndex++;
        if (_currentStoryIndex < StoryTexts.Length)
        {
            StoryTexts[_currentStoryIndex].SetActive(true);
            TypewriterEffect typewriterEffect = StoryTexts[_currentStoryIndex].GetComponent<TypewriterEffect>();
            typewriterEffect.StartTyping();
            typewriterEffect.OnTypingFinished += FinishTyping;
        }
    }

    IEnumerator CameraFocusPlayerOne()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(-15f, 5f, _camera.transform.position.z), time);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60, time);
            yield return null;
        }
        StartCoroutine(StartTutorialAfterDelay(StartTutorialTop));
    }

    IEnumerator CameraFocusPlayerTwo()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(-15f, -6f, _camera.transform.position.z), time);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60, time);
            yield return null;
        }
        StartCoroutine(StartTutorialAfterDelay(StartTutorialBottom));

        // StartCoroutine(StartGameAfterDelay());
    }

    IEnumerator CameraFocusCommon()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(-10f, -0.5f, _camera.transform.position.z), time);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 90, time);
            yield return null;
        }

        StartCoroutine(StartTutorialAfterDelay(StartTutorialCommon));
    }

    private IEnumerator StartTutorialAfterDelay(Action p)
    {
        yield return new WaitForSeconds(1f);
        p?.Invoke();
    }

    void StartTutorialTop()
    {
        if (TutorialTopTexts == null)
        {
            return;
        }
        if (_currentTutorialTopIndex == 0)
        {
            TutorialTopTexts[_currentTutorialTopIndex].SetActive(true);
        }

        if (HasFinishT1())
        {
            TutorialTopTexts[0].SetActive(false);

            _currentTutorialTopIndex++;

            TutorialTopTexts[1].SetActive(true);

            if (_currentTutorialTopIndex > 1 && _hasPressSpaceTwice)
            {
                foreach (var text in TutorialTopTexts)
                {
                    text.SetActive(false);
                }
                _isFinishTutorialTop = true;
            }
        }
    }

    void StartTutorialBottom()
    {
        if (TutorialBottomTexts == null)
        {
            return;
        }
        if (_currentTutorialBottomIndex == 0)
        {
            TutorialBottomTexts[_currentTutorialBottomIndex].SetActive(true);
        }

        if (_hasPressLeftShift)
        {
            _isFinishTutorialBottom = true;
        }
    }

    void StartTutorialCommon()
    {
        if (TutorialCommonTexts == null)
        {
            return;
        }
        if (_currentTutorialCommonIndex == 0)
        {
            TutorialBottomTexts[_currentTutorialCommonIndex].SetActive(false);
            TutorialCommonTexts[_currentTutorialCommonIndex].SetActive(true);
        }

        if (_hasPressQ && _currentTutorialCommonIndex == 0)
        {
            TutorialCommonTexts[_currentTutorialCommonIndex].SetActive(false);
            _currentTutorialCommonIndex++;
            TutorialCommonTexts[_currentTutorialCommonIndex].SetActive(true);
        }


        if (_hasPressX && _currentTutorialCommonIndex == 1)
        {
            TutorialCommonTexts[_currentTutorialCommonIndex].SetActive(false);
            _currentTutorialCommonIndex++;
            TutorialCommonTexts[_currentTutorialCommonIndex].SetActive(true);
        }

        if (_hasPressE && _currentTutorialCommonIndex == 2)
        {
            _isFinishTutorialCommon = true;
        }
    }
    private IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(2);
        StartGame();
    }

    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    //handle press "A,D and Space" to skip story
    public bool HasFinishT1()
    {
        Debug.Log(_hasPressA + " " + _hasPressD + " " + _hasPressSpace);
        return _hasPressA && _hasPressD && _hasPressSpace;
    }


}
