using System;
using System.Collections;
using UnityEngine;

public class ScreenLights : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private Rover rover;
    [SerializeField] private Ship ship;
    [SerializeField] private Light powerLight;
    [SerializeField] private Light errorLight;
    [SerializeField] private Renderer powerLED;
    [SerializeField] private Renderer errorLED;
    [SerializeField] private float blinkInterval = 0.5f;

    private Color powerEmissiveColor;
    private Color errorEmissiveColor;

    private Coroutine blinkCoro = null;

    private bool _power;
    public bool Power
    {
        get => _power;
        set
        {
            _power = value;
            powerLight.enabled = _power;
            powerLED.material.SetColor(EmissionColor, value ? powerEmissiveColor : Color.black);
        }
    }

    private bool _errorBlink;
    public bool ErrorBlink
    {
        get => _errorBlink;
        set
        {
            _errorBlink = value;
            errorLight.enabled = _errorBlink;
            errorLED.material.SetColor(EmissionColor, value ? errorEmissiveColor : Color.black);
        }
    }

    private bool _error;
    public bool Error
    {
        get => _error;
        set
        {
            if (Error == value) return;
            _error = value;
            if (value)
            {
                blinkCoro = StartCoroutine(BlinkCoro());
            }
            else
            {
                if (blinkCoro != null)
                {
                    StopCoroutine(blinkCoro);
                }
                blinkCoro = null;
                ErrorBlink = false;
            }
        }
    }

    private void Start()
    {
        powerEmissiveColor = powerLED.material.GetColor(EmissionColor);
        errorEmissiveColor = errorLED.material.GetColor(EmissionColor);
        
        _error = true;
        Error = false;

        rover.Status.ChangedEvent.AddListener(OnStatusChanged);
        ship.Status.ChangedEvent.AddListener(OnStatusChanged);

        Power = false;
        UpdateError();
    }

    void OnStatusChanged(){
        UpdateError();
    }

    void UpdateError(){
        bool allOk = rover.Status.AllOk;
        if(allOk){
            Error = false;
            AudioManager.Instance.Stop("Alert");
        }
        else {
            AudioManager.Instance.Play("Alert");
            Error = true;
        }
    }

    IEnumerator BlinkCoro()
    {
        ErrorBlink = true;
        while (true)
        {
            ErrorBlink = !ErrorBlink;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
