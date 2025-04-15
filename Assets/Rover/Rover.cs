using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


[System.Serializable]
public class RoverStatus
{
    [SerializeField] bool _power = true;
    [SerializeField] bool _comms = true;

    public UnityEvent ChangedEvent = new();

    public bool Power
    {
        get => _power;
        set
        {
            if (Power != value)
            {
                _power = value;
                ChangedEvent.Invoke();
            }
        }
    }

    public bool Comms
    {
        get => _comms;
        set
        {
            if (Comms != value)
            {
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
    [SerializeField] float brokenLightIntensity = 10;
    [SerializeField] private float lightInterpolateMult = 100f;
    [SerializeField] private new Transform camera;
    [SerializeField] private float cameraShakeAmount = 0.1f;
    [SerializeField] private GameObject jumpscareSprite;
    [System.NonSerialized] public bool InCamera = false;
    private float normalLightIntensity;
    float lightIntensity = 1.0f;

    int resourcePos => theResource.currentPosition;
    public bool repairsNeeded => !Status.AllOk;
    List<bool> puzzles = new List<bool> { false, false, false, false, false, false, false, false };
    int currentPuzzle = 0;
    bool powerTut = false;
    bool commsTut = false;

    private Vector3 cameraLocalPos;

    private void Awake()
    {
        normalLightIntensity = roverLight.intensity;
        cameraLocalPos = camera.localPosition;
    }

    void Update()
    {
        // adjusting light with no power
        // Gradually restore to full brightness (1.0)
        // Gradually reduce intensity toward 0.1
        roverLight.intensity = Mathf.MoveTowards(
            roverLight.intensity,
            !Status.Power ? brokenLightIntensity : normalLightIntensity,
            Time.deltaTime * lightInterpolateMult
        );

        jumpscareSprite.SetActive(puzzles[4] && !puzzles[5] && repairsNeeded);
        AudioManager.Instance.Mute("JumpscareSound", !(puzzles[4] && !puzzles[5] && repairsNeeded && InCamera));

        if (!Status.Power)
        {
            return;
        }

        // Convert the input direction to movement and rotation
        var moveDirection = transform.forward * MoveDir.y; // Forward/backward movement
        var rotation = MoveDir.x; // Left/right rotation


        // Apply forwards-backwards movement
        if (agent)
        {
            if(moveDirection != Vector3.zero)
            {
                agent.Move(moveDirection * (Time.deltaTime * agent.speed));
                ApplyScreenshake();
            }
        }

        // Apply rotation
        transform.Rotate(transform.up, rotation * Time.deltaTime * rotationSpeed); // Adjust rotation speed as needed

        BreakRover();
    }

    void ApplyScreenshake()
    {
        camera.localPosition = cameraLocalPos + new Vector3(
            Random.Range(-1, 1),
            Random.Range(-1, 1),
            Random.Range(-1, 1)
        ) * cameraShakeAmount;
    }

    private void BreakRover()
    {
        // tutorials
        if ((powerTut == false) && (roverTransform.position.z > 105) && (repairsNeeded == false))
        {
            powerTut = true;
            Status.Power = false;
        }
        if ((commsTut == false) && (roverTransform.position.z > 110)  && (repairsNeeded == false))
        {
            commsTut = true;
            Status.Comms = false;
        }

        // actual breakdowns
        if ((resourcePos == 0) & (repairsNeeded == false)) // start
        {
            if ((roverTransform.position.z > 230) & (puzzles[0] == false))
            {
                // activate comms puzzle W
                Status.Comms = false;
                puzzles[0] = true;
                currentPuzzle++;
            }
            else if (roverTransform.position.z > 160 & (puzzles[1] == false))
            {
                // activate power puzzle A
                Status.Power = false;
                puzzles[1] = true;
                currentPuzzle++;
            }
            else if (Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50 &
                     (puzzles[2] == false))
            {
                // activate power puzzle B
                Status.Power = false;
                puzzles[2] = true;
                currentPuzzle++;
            }
        }

        else if ((resourcePos == 1) & (repairsNeeded == false)) // top right
        {
            if ((roverTransform.position.x > 240) & (puzzles[3] == false))
            {
                // activate comms puzzle Y
                // then show jumpscare
                Status.Comms = false;
                puzzles[3] = true;
                currentPuzzle++;
            }
            else if (InCamera && roverTransform.position.x > 140 & (puzzles[4] == false))
            {
                // activate power puzzle X
                Status.Power = false;
                puzzles[4] = true;
                currentPuzzle++;
            }
            else if (Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50 &
                     (puzzles[5] == false))
            {
                // activate power puzzle C
                Status.Comms = false;
                puzzles[5] = true;
                currentPuzzle++;
            }
        }

        else if ((resourcePos == 2) & (repairsNeeded == false)) // top right
        {
            // D — Z < 205
            //     // Z — Z <130 ish
            if ((Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50) & (puzzles[6] == false))
            {
                // activate comms puzzle Z
                Status.Comms = false;
                puzzles[6] = true;
                currentPuzzle++;
            }
        }
        else if ((resourcePos == 3) & repairsNeeded == false) // middle right
        {
            if ((Vector3.Distance(theResource.transform.position, roverTransform.transform.position) <= 50) & (puzzles[7] == false))
            {
                // activate power puzzle D
                Status.Power = false;
                puzzles[7] = true;
                currentPuzzle++;
            }
        }
    }
}