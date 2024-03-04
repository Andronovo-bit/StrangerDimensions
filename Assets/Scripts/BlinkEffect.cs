using System.Collections;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public RectTransform upperBox;
    public RectTransform lowerBox;
    public float speed = 0.70f;
    public int blinkTimes = 3;
    public bool endClosing = false;
    public AnimationCurve blinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Added for smoothness

    private int currentBlink = 0; // Start from 0 to count properly

    void OnEnable()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (currentBlink < blinkTimes)
        {
            // Increment at the start to simplify logic
            currentBlink++;

            // Open eyelids
            yield return ScaleEyelids(0, true);

            // Close eyelids
            yield return ScaleEyelids(70, false);

            // Check if we want to end the blink closing the eyes on the last blink
            if (currentBlink == blinkTimes && endClosing)
            {
                // Adjust the logic to keep the eyelids closed or perform any final action
                upperBox.localScale = new Vector3(1, 0, 1);
                lowerBox.localScale = new Vector3(1, 0, 1);
            }
        }
        if (currentBlink == blinkTimes)
            yield return ScaleEyelids(0, true);

    }

    private IEnumerator ScaleEyelids(float targetScaleY, bool opening)
    {
        float elapsedTime = 0;
        float startScaleY = upperBox.localScale.y;

        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            float duration = elapsedTime / speed;
            float curveValue = blinkCurve.Evaluate(duration); // Use the curve for smoothness

            float newScaleY = Mathf.Lerp(startScaleY, targetScaleY, curveValue);
            upperBox.localScale = new Vector3(1, newScaleY, 1);
            lowerBox.localScale = new Vector3(1, newScaleY, 1);

            yield return null;
        }

        // Ensure the final scale is set precisely at the end of the animation
        upperBox.localScale = new Vector3(1, targetScaleY, 1);
        lowerBox.localScale = new Vector3(1, targetScaleY, 1);
    }
}