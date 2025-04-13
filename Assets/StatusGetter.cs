using UnityEngine;

public class StatusGetter : MinigameBase
{
    [SerializeField] ShipStats shipStats;
    [SerializeField] TheResource resourceStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMPro.TMP_Text text;

    string roverName = "";
    bool comms = true;
    bool power = true;

    void Start()
    {
       

    }


    // Update is called once per frame
    void Update()
    {
        string com_text = comms ? "GOOD" : "POOR";
        int pow_text = power ? Random.Range(100, 116) : Random.Range(45, 56);

        text.text = 
    $@"STATUS: 
    
    Distance from resource: {resourceStats.roverResourceDist}m
    Direction of resource: {resourceStats.roverResourceDir}
    
    Distance from ship: {shipStats.roverShipDist}m
    Direction of ship: {shipStats.roverShipDir}

    Signal Strength: {com_text}
    Power: {pow_text}W ";
    }
}
