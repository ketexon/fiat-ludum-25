using UnityEngine;

public class ShipStats : MonoBehaviour
{
    [SerializeField] Transform rover;
    [SerializeField] Transform ship;

    public string roverShipDir = "S";
    public float roverShipDist = 0.01f;

    void Update()
    {
        float distanceToShip = Vector3.Distance(rover.position, ship.position);
        roverShipDist = distanceToShip;

        Vector3 directionToShip = ship.position - rover.position;
        directionToShip.y = 0; // Keep it flat

        float angle = Mathf.Atan2(directionToShip.x, directionToShip.z) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f; // Normalize to 0-360 degrees

        roverShipDir = GetCompassDirection(angle);
    }

    string GetCompassDirection(float angle)
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
