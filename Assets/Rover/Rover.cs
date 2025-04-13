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
    [SerializeField] Light roverLight;
    int resourcePos => theResource.currentPosition;
    public bool repairsNeeded = false;
    List<bool> puzzles = new List<bool> { false, false, false, false, false, false, false, false};
    int currentPuzzle = 0;
    float lightIntensity = 1.0f;
    void Update()
    {
        // adjusting light with no power
        if (!Status.Power)
        {
            // Gradually reduce intensity toward 0.1
            lightIntensity = Mathf.MoveTowards(lightIntensity, 0.1f, Time.deltaTime * 2f); 
        }
        else
        {
            // Gradually restore to full brightness (1.0)
            lightIntensity = Mathf.MoveTowards(lightIntensity, 1.0f, Time.deltaTime * 2f);
        }
        roverLight.intensity = lightIntensity;
        
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
            if ((roverTransform.position.z > 230) & (puzzles[0] == false))
            {
                // activate comms puzzle W
                repairsNeeded = true;
                Status.Comms = false;
                puzzles[0] = true;
                currentPuzzle ++;
            }
            else if (roverTransform.position.z > 140 & (puzzles[1] == false))
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

        else if ((resourcePos == 1) & (repairsNeeded == false)) // top right
        {
            if ((roverTransform.position.x > 140) & (puzzles[3] == false))
            {
                // activate comms puzzle Y
                repairsNeeded = true;
                Status.Comms = false;
                puzzles[3] = true;
                currentPuzzle ++;
            }
            else if (roverTransform.position.x > 40 & (puzzles[4] == false))
            {
                // activate comms puzzle X
                repairsNeeded = true;
                Status.Comms = false;
                puzzles[4] = true;
                currentPuzzle ++;
            }
            else if (Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50 & (puzzles[5] == false))
            {
                // activate power puzzle C
                repairsNeeded = true;
                Status.Power = false;
                puzzles[5] = true;
                currentPuzzle ++;
            }
        }

        else if ((resourcePos == 2) & (repairsNeeded == false)) // top right
        {
            // D — Z < 205
        //     // Z — Z <130 ish
            if ((roverTransform.position.z < 130) & (puzzles[6] == false))
            {
                // activate comms puzzle Z
                repairsNeeded = true;
                Status.Comms = false;
                puzzles[6] = true;
                currentPuzzle ++;
            }
            else if (roverTransform.position.z < 205 & (puzzles[7] == false))
            {
                // activate power puzzle D
                repairsNeeded = true;
                Status.Power = false;
                puzzles[7] = true;
                currentPuzzle ++;
            }
        }

    }
}
