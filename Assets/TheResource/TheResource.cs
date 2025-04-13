using UnityEngine;
using System.Collections.Generic;

public class TheResource : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // making a list of all the places to iterate through
    [SerializeField] List<Transform> places = new();    
    [SerializeField] Transform Rover;

    int currentPosition = 0;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Rover.position) < 10.0f)
        {
            Debug.Log("TheResource is close to the Rover.");
            if (currentPosition <= places.Count - 1)
            {
                currentPosition++;
                transform.position = places[currentPosition].transform.position;
                Debug.Log("The Resource has moved to the next place: " + transform.position);
            }
        }
        else
        {
            Debug.Log("TheResource is not close to the Rover.");
        }
    }
}
