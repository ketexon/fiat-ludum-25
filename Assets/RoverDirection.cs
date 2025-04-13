using UnityEngine;

public class RoverDirection : MonoBehaviour
{
    public string roverDir = "N";

    void Update()
    {
        getFacingDirection();
    }

    void getFacingDirection()
    {
        Vector3 forward = transform.forward;
        forward.y = 0; // Ignore vertical tilt (only care about flat ground)
        forward.Normalize();

        float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f; // Normalize between 0–360

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

        roverDir = direction;
    }
}