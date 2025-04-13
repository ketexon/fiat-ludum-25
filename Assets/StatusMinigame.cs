using UnityEngine;
using System.Collections;
public class StatusMinigame : MinigameBase
{
    [SerializeField] ShipStats shipStats;
    [SerializeField] TheResource resourceStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] Rover rover;
    bool Comms => rover.Status.Comms;
    bool Power => rover.Status.Power;
    int pow_text = 100;

    void Start()
    {
       StartCoroutine(outputPower());
    }

    // Update is called once per frame
    void Update()
    {
        string com_text = Comms ? "GOOD" : "POOR";

        text.text =
    $@"STATUS: {Terminal.State.RoverName}

    Distance from resource: {resourceStats.roverResourceDist}m
    Direction of resource: {resourceStats.roverResourceDir}

    Distance from ship: {shipStats.roverShipDist}m
    Direction of ship: {shipStats.roverShipDir}

    Signal Strength: {com_text}
    Power: {pow_text}W ";
    }

    IEnumerator outputPower()
    {
        while(true)
        {
            pow_text = Power ? Random.Range(100, 116) : Random.Range(0, 10);
            yield return new WaitForSeconds(1f);
        }
    }
}
