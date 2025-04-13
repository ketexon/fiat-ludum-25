using UnityEngine;
using System.Collections;
public class StatusMinigame : MinigameBase
{
    [SerializeField] Transform ship;
    [SerializeField] Transform theResource;
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

        var resourceDistance = Vector3.Distance(theResource.position, rover.transform.position);
        var resourceDir = DirTo(theResource);

        var shipDistance = Vector3.Distance(ship.position, rover.transform.position);
        var shipDir = DirTo(ship);

        text.text =
    $@"STATUS: {Terminal.State.RoverName}

    Distance from resource: {resourceDistance}m
    Direction of resource: {resourceDir}

    Distance from ship: {shipDistance}m
    Direction of ship: {shipDir}

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

    string DirTo(Transform t) {
        var angle = Vector3.SignedAngle(Vector3.forward, t.position - rover.transform.position, Vector3.up);
        return GetCompassDirection(angle);
    }

    static string GetCompassDirection(float angle)
    {
        if (angle >= -22.5f && angle < 22.5f)
            return "N";
        else if (angle >= 22.5f && angle < 67.5f)
            return "NE";
        else if (angle >= 67.5f && angle < 112.5f)
            return "E";
        else if (angle >= 112.5f && angle < 157.5f)
            return "SE";
        else if (angle >= 157.5f || angle < -157.5f)
            return "S";
        else if (angle >= -157.5f && angle < -112.5f)
            return "SW";
        else if (angle >= -112.5f && angle < -67.5f)
            return "W";
        else if (angle >= -67.5f && angle < -22.5f)
            return "NW";
        else
            return "Unknown";
    }
}
