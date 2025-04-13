using UnityEngine;

public class StatusGetter : MonoBehaviour
{
    [SerializeField] ShipStats shipStats;
    [SerializeField] TheResource resourceStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ship is " + shipStats.roverShipDir + " of Rover.");
        Debug.Log("Distance to Ship: " + shipStats.roverShipDist);

        Debug.Log("Resource is " + resourceStats.roverResourceDir + " of Rover.");
        Debug.Log("Distance to Resource: " + resourceStats.roverResourceDist);
        
    }
}
