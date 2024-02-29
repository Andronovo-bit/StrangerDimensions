using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerTop;
    public GameObject playerBottom;
    public GameObject mainCamera;
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
        Vector3 tempPosition = playerTop.transform.position;
        playerTop.transform.position = playerBottom.transform.position;
        playerBottom.transform.position = tempPosition;
    }

    // Optional: Implement a cooldown for swapping to prevent spamming
    IEnumerator SwapCooldown()
    {
        isSwapReady = false;
        yield return new WaitForSeconds(1); // Cooldown period of 1 second
        isSwapReady = true;
    }

    //change the camera position +5 with delta time
    public IEnumerator ChangeCameraPosition()
    {
        float elapsedTime = 0;
        Vector3 startingPos = mainCamera.transform.position;
        Vector3 endPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5, mainCamera.transform.position.z);
        while (elapsedTime < 1)
        {
            mainCamera.transform.position = Vector3.Lerp(startingPos, endPos, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


}
