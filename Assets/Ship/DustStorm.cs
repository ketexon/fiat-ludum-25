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
            WarningEvent.Invoke();
            yield return new WaitForSeconds(warningTime);
            StartedEvent.Invoke();
            yield return new WaitForSeconds(duration);
            EndedEvent.Invoke();
        }
    }
}