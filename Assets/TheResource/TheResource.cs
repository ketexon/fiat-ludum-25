using UnityEngine;
using System.Collections.Generic;

public class TheResource : MonoBehaviour
{
    [SerializeField] List<Transform> places = new();
    [SerializeField] Transform Rover;
    [SerializeField] float moveSpeed = 12.0f; // Speed at which the resource moves

    int currentPosition = 0;
    bool isMoving = false;

    float roverResourceDist = 0;
    string roverResourceDir = "";

    void Update()
    {
        float distanceToRover = Vector3.Distance(transform.position, Rover.position);

        roverResourceDir = getDirection();
        roverResourceDist = distanceToRover;

        if (!isMoving && distanceToRover < 50.0f)
        {
            Debug.Log("TheResource is close to the Rover. Now it's moving.");
            if (currentPosition < places.Count - 1)
            {
                currentPosition++;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            MoveTowardsNextPlace();
        }
    }

    void MoveTowardsNextPlace()
    {
        Transform targetPlace = places[currentPosition];
        transform.position = Vector3.MoveTowards(transform.position, targetPlace.position, moveSpeed * Time.deltaTime);

        // If the resource reaches the target
        if (Vector3.Distance(transform.position, targetPlace.position) < 0.01f)
        {
            transform.position = targetPlace.position; // Snap exactly to position
            isMoving = false; // Stop moving
            Debug.Log("The Resource has arrived at: " + transform.position);
        }
    }

    string getDirection()
    {
        Vector3 toResource = transform.position - Rover.position;
        float angle = Mathf.Atan2(toResource.x, toResource.z) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f; // Normalize to 0-360 degrees

        string direction = "";

        if (angle >= 337.5f || angle < 22.5f)
            direction = "N";
        else if (angle >= 22.5f && angle < 67.5f)
            direction = "NE";
        else if (angle >= 67.5f && angle < 112.5f)
            direction = "E";
        else if (angle >= 112.5f && angle < 157.5f)
            direction = "SE";
        else if (angle >= 157.5f && angle < 202.5f)
            direction = "S";
        else if (angle >= 202.5f && angle < 247.5f)
            direction = "SW";
        else if (angle >= 247.5f && angle < 292.5f)
            direction = "W";
        else if (angle >= 292.5f && angle < 337.5f)
            direction = "NW";

        return direction;
    }
}
