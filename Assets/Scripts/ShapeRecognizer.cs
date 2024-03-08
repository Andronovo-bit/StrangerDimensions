using UnityEngine;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public class ShapeRecognizer : MonoBehaviour
{
    public LineRenderer lineRendererPrefab;
    private List<Point> points = new List<Point>();
    private bool isDrawing = false;
    private int strokeId = -1;
    private List<Gesture> trainingSet = new List<Gesture>();

    void Start()
    {
        LoadTrainingSet();
    }

    void LoadTrainingSet()
    {
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || IsTouchBegin())
        {
            StartDrawing();
        }
        else if ((Input.GetMouseButton(0) || IsTouchContinue()) && isDrawing)
        {
            ContinueDrawing();
        }
        else if ((Input.GetMouseButtonUp(0) || IsTouchEnd()) && isDrawing)
        {
            EndDrawing();
        }
        else if (Input.GetMouseButtonDown(1)) // Right-click to cancel
        {
            CancelDrawing();
        }
    }

    bool IsTouchBegin()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    bool IsTouchContinue()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
    }

    bool IsTouchEnd()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
    }

    void StartDrawing()
    {
        isDrawing = true;
        points.Clear();
        strokeId = -1;
    }

    void ContinueDrawing()
    {
        Vector2 currentPosition = GetMouseWorldPosition();
        if (points.Count % 2 == 0)
        {
            strokeId++;
        }
        points.Add(new Point(currentPosition.x, -currentPosition.y, strokeId));
        lineRendererPrefab.positionCount = points.Count;
        lineRendererPrefab.SetPosition(points.Count - 1, currentPosition);
    }

    void EndDrawing()
    {
        isDrawing = false;
        if (points.Count > 2)
            RecognizeShape();
    }

    void CancelDrawing()
    {
        isDrawing = false;
        points.Clear();
        lineRendererPrefab.positionCount = 0;
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10));
        return new Vector2(worldPosition.x, worldPosition.y);
    }

    void RecognizeShape()
    {
        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        Debug.Log("Gesture ID: " + gestureResult.GestureClass + " Score: " + gestureResult.Score);
    }
}