using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player playerTop;
    public Player playerBottom;
    public CinemachineVirtualCamera mainCamera;
    public GameObject world;
    private bool isSwapReady = true; // Ensure we don't swap positions too frequently

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwapPositions();
            StartCoroutine(SwapCooldown()); // Optional: Implement a cooldown for swapping
        }
    }

    void SwapPositions()
    {
        //Vector3 tempPosition = playerTop.transform.position;
        //playerTop.transform.position = playerBottom.transform.position;
        //playerBottom.transform.position = tempPosition;
        //playerBottom.SwitchWorld();
        //playerTop.SwitchWorld();
        //change cinemachine follow target
        ChangeCameraPosition();
        ActivateSplitScreen(true);
    }

    void ChangeCameraPosition()
    {
        mainCamera.Follow = mainCamera.Follow == playerTop.transform ? playerBottom.transform : playerTop.transform;
        world.transform.rotation = world.transform.rotation == Quaternion.Euler(0, 0, 0) ? Quaternion.Euler(0, 180, 180) : Quaternion.Euler(0, 0, 0);
        playerBottom.SwitchWorld();
        playerTop.SwitchWorld();
    }

    // Optional: Implement a cooldown for swapping to prevent spamming
    IEnumerator SwapCooldown()
    {
        isSwapReady = false;
        yield return new WaitForSeconds(1); // Cooldown period of 1 second
        isSwapReady = true;
    }

    //change the camera position +5 with delta time
    //public IEnumerator ChangeCameraPosition()
    //{
    //    //float elapsedTime = 0;
    //    //Vector3 startingPos = mainCamera.transform.position;
    //    //Vector3 endPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5, mainCamera.transform.position.z);
    //    //while (elapsedTime < 1)
    //    //{
    //    //    mainCamera.transform.position = Vector3.Lerp(startingPos, endPos, elapsedTime);
    //    //    elapsedTime += Time.deltaTime;
    //    //    yield return null;
    //    //}
    //}

    public void ActivateSplitScreen(bool isActive)
    {
        //change camera Bias Y value
    }


}
