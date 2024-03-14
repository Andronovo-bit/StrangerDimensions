using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Player playerTop;
    [SerializeField] private Player playerBottom;
    [SerializeField] private GameObject terrainTop;
    [SerializeField] private GameObject terrainBottom;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private GameObject blinkEffect; // Reference to the BlinkEffect script
    [SerializeField] private GameObject world;

    private Player _mainPlayer => PlayerPrefs.GetInt("PlayerType") == 0 ? playerTop : playerBottom;
    private bool isSwapReady = true; // Ensure we don't swap positions too frequently

    private void Start()
    {
        if (PlayerPrefs.GetInt("PlayerType") == (int)PlayerType.PlayerBottom && SceneManager.GetActiveScene().buildIndex != 1)
        {
            SwapPositions(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isSwapReady)
        {
            SwapPositions(true);
            StartCoroutine(SwapCooldown()); // Optional: Implement a cooldown for swapping
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //get all players   
            var players = FindObjectsOfType<Player>();
            //except the main player
            foreach (var player in players)
            {
                if (player != _mainPlayer)
                {
                    player.transform.position = new Vector3(_mainPlayer.transform.position.x, player.transform.position.y, _mainPlayer.transform.position.z);
                }
            }

        }
    }

    private void SwapPositions(bool wait)
    {
        StartCoroutine(ChangeCameraPosition(wait));
        ActivateSplitScreen(true);
    }

    private IEnumerator ChangeCameraPosition(bool wait)
    {
        if (wait)
        {
            blinkEffect.SetActive(true);

            BlinkEffect _blinkEffect = blinkEffect.GetComponent<BlinkEffect>();
            _blinkEffect.enabled = true;

            Debug.Log("Blinking");
            Debug.Log("FixedDeltaTime: " + Time.fixedDeltaTime);

            yield return new WaitForSeconds(Time.fixedDeltaTime * 75); // Wait for the blink effect to finish
        }

        mainCamera.Follow = mainCamera.Follow == playerTop.transform ? playerBottom.transform : playerTop.transform;
        world.transform.rotation = world.transform.rotation == Quaternion.Euler(0, 0, 0) ? Quaternion.Euler(0, 180, 180) : Quaternion.Euler(0, 0, 0);
        playerBottom.SwitchWorld();
        playerTop.SwitchWorld();

        //If the character is stuck somewhere, send it to the nearest previous free space


        // bool isFollowingTopPlayer = mainCamera.Follow == playerTop.transform;
        // terrainTop.SetActive(isFollowingTopPlayer);
        // terrainBottom.SetActive(!isFollowingTopPlayer);
    }

    // Optional: Implement a cooldown for swapping to prevent spamming
    private IEnumerator SwapCooldown()
    {
        isSwapReady = false;
        yield return new WaitForSeconds(1); // Cooldown period of 1 second
        isSwapReady = true;
    }

    public void ActivateSplitScreen(bool isActive)
    {
        //change camera Bias Y value
    }
}