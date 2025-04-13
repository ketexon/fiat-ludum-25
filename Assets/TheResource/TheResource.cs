using UnityEngine;
using System.Collections.Generic;

public class TheResource : MonoBehaviour
{
    [SerializeField] List<Transform> places = new();
    [SerializeField] Transform Rover;
    [SerializeField] float moveSpeed = 12.0f; // Speed at which the resource moves

    int currentPosition = 0;
    bool isMoving = false;

    void Update()
    {
        float distanceToRover = Vector3.Distance(transform.position, Rover.position);

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
}
