using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Rover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float rotationSpeed;
    Vector2 dir = Vector2.zero;

    void Start()
    {
        
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
        

    }


    void Update()
    {
        // Convert the input direction to movement and rotation
        Vector3 moveDirection = transform.forward * dir.y; // Forward/backward movement
        float rotation = dir.x; // Left/right rotation

        // Apply forwards-backwards movement
        if (agent != null)
        {
            agent.Move(moveDirection * Time.deltaTime * agent.speed);
        }

        // Apply rotation
        transform.Rotate(transform.up, rotation * Time.deltaTime * rotationSpeed); // Adjust rotation speed as needed
    }
}
