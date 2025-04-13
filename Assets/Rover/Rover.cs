using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[System.Serializable]
public class RoverStatus {
    public bool Power = true;
    public bool Comms = true;
}

public class Rover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public RoverStatus Status;

    [System.NonSerialized] public Vector2 MoveDir = Vector2.zero;

    void Update()
    {
        // Convert the input direction to movement and rotation
        Vector3 moveDirection = transform.forward * MoveDir.y; // Forward/backward movement
        float rotation = MoveDir.x; // Left/right rotation

        // Apply forwards-backwards movement
        if (agent != null)
        {
            agent.Move(moveDirection * Time.deltaTime * agent.speed);
        }

        // Apply rotation
        transform.Rotate(transform.up, rotation * Time.deltaTime * rotationSpeed); // Adjust rotation speed as needed
    }
}
