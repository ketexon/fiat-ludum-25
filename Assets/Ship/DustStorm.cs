using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DustStorm : MonoBehaviour
{
    [SerializeField] private float delay = 40.0f;
    [SerializeField] private float warningTime = 10.0f;
    [SerializeField] private float duration = 10.0f;

    [System.NonSerialized] public UnityEvent WarningEvent = new();
    [System.NonSerialized] public UnityEvent StartedEvent = new();
    [System.NonSerialized] public UnityEvent EndedEvent = new();

    public bool Active { get; private set; } = false;
    public bool isWindy { get; private set; } = false;
    
    private Coroutine coro;
    
    private void OnEnable()
    {
        coro = StartCoroutine(DustStormCoro());
    }
    
    private void OnDisable()
    {
        EndedEvent.Invoke();
        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }
    }

    IEnumerator DustStormCoro()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            AudioManager.Instance.Play("WindWarning");
            isWindy = true;
            WarningEvent.Invoke();
            yield return new WaitForSeconds(warningTime);
            Active = true;
            StartedEvent.Invoke();
            yield return new WaitForSeconds(duration);
            Active = false;
            EndedEvent.Invoke();
            isWindy = false;
        }
    }
}