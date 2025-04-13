using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform rover;
    Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir.z = -rover.eulerAngles.y;
        transform.localEulerAngles = dir;
    }
}
