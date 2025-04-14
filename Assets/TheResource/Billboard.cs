using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    private void Update()
    {
        var dir = camera.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
    }
}
