using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;


[System.Serializable]
public class RoverStatus {
    [SerializeField] bool _power = true;
    [SerializeField] bool _comms = true;

    public UnityEvent ChangedEvent = new();

    public bool Power {
        get => _power;
        set {
            if(Power != value){
                _power = value;
                ChangedEvent.Invoke();
            }
        }
    }
    public bool Comms {
        get => _comms;
        set {
            if(Comms != value){
                _comms = value;
                ChangedEvent.Invoke();
            }
        }
    }

    public bool AllOk => Power && Comms;
}

public class Rover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public RoverStatus Status;
    [SerializeField] Transform roverTransform;
    [SerializeField] TheResource theResource;
    [System.NonSerialized] public Vector2 MoveDir = Vector2.zero;
    int resourcePos => theResource.currentPosition;
    public bool repairsNeeded = false;
    List<bool> puzzles = new List<bool> { false, false, false, false, false, false, false, false};
    int currentPuzzle = 0;


    void Update()
    {
        if(!Status.Power){
            return;
        }

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

        breakRover();
    }

    void breakRover()
    {   
        if ((resourcePos == 0) & (repairsNeeded == false)) // start
        {
            if ((roverTransform.position.z > 240) & (puzzles[0] == false))
            {
                // activate comms puzzle W
                repairsNeeded = true;
                Status.Comms = false;
                puzzles[0] = true;
                currentPuzzle ++;
            }
            else if (roverTransform.position.z > 175 & (puzzles[1] == false))
            {
                // activate power puzzle A
                repairsNeeded = true;
                Status.Power = false;
                puzzles[1] = true;
                currentPuzzle ++;
            }
            else if (Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50 & (puzzles[2] == false))
            {
                // activate power puzzle B
                repairsNeeded = true;
                Status.Power = false;
                puzzles[2] = true;
                currentPuzzle ++;
            }
        }

        // else if (resourcePos == 1) // top right
        // {
        //     // X — X > 40 
        //     // Y — X > 140
        //     // MOVE : C — 50 distance
        // }

        // else if (resourcePos == 2) // outside cave
        // {
        //     // D — Z < 205
        //     // Z — Z <130 ish
        // }   
    }
}
