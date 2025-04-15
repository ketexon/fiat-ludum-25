using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Light shipLight;
    [SerializeField] Ship ship;
    public bool isWindy => ship.dustStorm.isWindy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shipLight.intensity = Random.Range(4, 8);
        if (isWindy)
        {
            shipLight.intensity = Random.Range(1, 15);
        } 
    }
}