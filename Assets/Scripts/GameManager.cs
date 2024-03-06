using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Player playerTop;
    [SerializeField] private Player playerBottom;
    [SerializeField] private GameObject terrainTop;
    [SerializeField] private GameObject terrainBottom;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private GameObject blinkEffect; // Reference to the BlinkEffect script
    [SerializeField] private GameObject world;

    private bool isSwapReady = true; // Ensure we don't swap positions too frequently

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isSwapReady)
        {
            SwapPositions();
            StartCoroutine(SwapCooldown()); // Optional: Implement a cooldown for swapping
        }
    }

    private void SwapPositions()
    {
        StartCoroutine(ChangeCameraPosition());
        ActivateSplitScreen(true);
    }

    private IEnumerator ChangeCameraPosition()
    {
        blinkEffect.SetActive(true);

        BlinkEffect _blinkEffect = blinkEffect.GetComponent<BlinkEffect>();
        _blinkEffect.enabled = true;

        Debug.Log("Blinking");
        Debug.Log("FixedDeltaTime: " + Time.fixedDeltaTime);

        yield return new WaitForSeconds(Time.fixedDeltaTime * 75); // Wait for the blink effect to finish

        mainCamera.Follow = mainCamera.Follow == playerTop.transform ? playerBottom.transform : playerTop.transform;
        world.transform.rotation = world.transform.rotation == Quaternion.Euler(0, 0, 0) ? Quaternion.Euler(0, 180, 180) : Quaternion.Euler(0, 0, 0);
        playerBottom.SwitchWorld();
        playerTop.SwitchWorld();

        //If the character is stuck somewhere, send it to the nearest previous free space


        bool isFollowingTopPlayer = mainCamera.Follow == playerTop.transform;
        terrainTop.SetActive(isFollowingTopPlayer);
        terrainBottom.SetActive(!isFollowingTopPlayer);
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