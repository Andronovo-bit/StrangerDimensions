using UnityEngine;

public class ShapeRecognizer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isDrawing = false;

    void Start()
    {
        lineRenderer.sortingLayerName = "Foreground";
        lineRenderer.sortingOrder = 1;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            Debug.Log("Position Count: " + lineRenderer.positionCount);
            EndDrawing();
            RecognizeShape();

        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelDrawing();
        }
    }

    void CancelDrawing()
    {
        isDrawing = false;
        lineRenderer.positionCount = 0;
    }

    void ContinueDrawing()
    {
        Vector2 currentPosition = GetMouseWorldPosition();
        AddLineRendererPosition(currentPosition);
    }

    void AddLineRendererPosition(Vector2 position)
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    void StartDrawing()
    {
        isDrawing = true;
        startPosition = GetMouseWorldPosition();
        SetLineRendererPosition(0, startPosition);
    }

    void EndDrawing()
    {
        isDrawing = false;
        endPosition = GetMouseWorldPosition();
        //SetLineRendererPosition(1, endPosition);
    }

    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void SetLineRendererPosition(int index, Vector2 position)
    {
        lineRenderer.positionCount = index + 1;
        lineRenderer.SetPosition(index, position);
    }

    void RecognizeShape()
    {
        Vector2 direction = endPosition - startPosition;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        if (IsHorizontalLine(angle))
        {
            Debug.Log("Horizontal Line");
        }
        else if (IsVerticalLine(angle))
        {
            Debug.Log("Vertical Line");
        }
        else if (IsUpwardDiagonal(angle))
        {
            Debug.Log("Upward Diagonal");
        }
        else if (IsDownwardDiagonal(angle))
        {
            Debug.Log("Downward Diagonal");
        }
        else if (IsUpwardArrow(angle))
        {
            Debug.Log("Upward Arrow");
        }
        else if (IsDownwardArrow(angle))
        {
            Debug.Log("Downward Arrow");
        }
    }

    bool IsHorizontalLine(float angle)
    {
        return Mathf.Abs(angle) < 30 || Mathf.Abs(angle) > 150;
    }

    bool IsVerticalLine(float angle)
    {
        return angle > 60 && angle < 120 || angle < -60 && angle > -120;
    }

    bool IsUpwardDiagonal(float angle)
    {
        return angle >= 30 && angle <= 60 || angle >= -150 && angle <= -120;
    }

    bool IsDownwardDiagonal(float angle)
    {
        return angle >= 120 && angle <= 150 || angle >= -60 && angle <= -30;
    }

    bool IsUpwardArrow(float angle)
    {
        // Bu, sembolün yukarı yönlü bir ok olup olmadığını kontrol etmek için basit bir yer tutucudur.
        return angle > -30 && angle < 30;
    }

    bool IsDownwardArrow(float angle)
    {
        // Bu, sembolün aşağı yönlü bir ok olup olmadığını kontrol etmek için basit bir yer tutucudur.
        return angle > 150 || angle < -150;
    }
}