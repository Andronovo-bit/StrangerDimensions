using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    public List<Transform> Waypoints;
    public float Speed = 2f;
    public float StoppingDistance = 0.2f;
    public bool Loop = false;

    private int _waypointIndex = 0;

    private void Update()
    {
        if (NoWaypoints())
        {
            return;
        }

        var targetPosition = GetTargetPosition();
        var distance = GetDistanceToTarget(targetPosition);

        if (IsCloseEnough(distance))
        {
            MoveToNextWaypoint();
        }
        else
        {
            MoveTowardsTarget(targetPosition);
        }
    }

    private bool NoWaypoints()
    {
        return Waypoints.Count == 0;
    }

    private Vector3 GetTargetPosition()
    {
        return Waypoints[_waypointIndex].position;
    }

    private float GetDistanceToTarget(Vector3 targetPosition)
    {
        var moveDirection = targetPosition - transform.position;
        return moveDirection.magnitude;
    }

    private bool IsCloseEnough(float distance)
    {
        return distance <= StoppingDistance;
    }

    private void MoveToNextWaypoint()
    {
        _waypointIndex++;
        if (_waypointIndex >= Waypoints.Count)
        {
            if (Loop)
            {
                _waypointIndex = 0;
            }
            else
            {
                enabled = false;
            }
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        var moveSpeed = Speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);
    }
}