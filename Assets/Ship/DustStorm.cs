using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DustStorm : MonoBehaviour
{
    enum State
    {
        Idle,
        Warning,
        Active,
    }
    
    [SerializeField] private float delay = 40.0f;
    [SerializeField] private float warningTime = 10.0f;
    [SerializeField] private float duration = 10.0f;

    [System.NonSerialized] public UnityEvent WarningEvent = new();
    [System.NonSerialized] public UnityEvent StartedEvent = new();
    [System.NonSerialized] public UnityEvent EndedEvent = new();

    public bool Active => state == State.Active;
    public bool IsWindy => state != State.Idle;
    private Coroutine coro;

    private float timeUntilChange = -1f;

    private State state = State.Active;
    
    private void OnEnable()
    {
        if (timeUntilChange < 0)
        {
            NextState();
        }

        AudioManager.Instance.Dim("WindWarning", false);
    }
    
    private void OnDisable()
    {
        EndedEvent.Invoke();
        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }
        if(AudioManager.Instance)
        {
            AudioManager.Instance.Dim("WindWarning", true);
        }
    }

    void Update()
    {
        timeUntilChange -= Time.deltaTime;
        if (timeUntilChange <= 0)
        {
            NextState();
        }
    }

    void NextState()
    {
        switch (state)
        {
            case State.Active:
                state = State.Idle;
                timeUntilChange = delay;
                EndedEvent.Invoke();
                break;
            case State.Idle:
                state = State.Warning;
                timeUntilChange = warningTime;
                WarningEvent.Invoke();
                AudioManager.Instance.Play("WindWarning");
                break;
            case State.Warning:
                state = State.Active;
                timeUntilChange = duration;
                StartedEvent.Invoke();
                break;
        }
    }
}