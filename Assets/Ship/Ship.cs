using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ShipStatus
{
    float _oxygen = 1f;

    public float Oxygen
    {
        get => _oxygen;
        set
        {
            _oxygen = Mathf.Clamp01(value);
            OxygenWarning = _oxygen < 0.3f;
        }
    }

    private bool _oxygenWarning = false;
    public bool OxygenWarning
    {
        get => _oxygenWarning;
        set
        {
            if (value == _oxygenWarning) return;
            _oxygenWarning = value;
            ChangedEvent.Invoke();
        }
    }

    bool _dustStorm = false;
    public bool DustStorm
    {
        get => _dustStorm;
        set
        {
            if (value == _dustStorm) return;
            _dustStorm = value;
            ChangedEvent.Invoke();
        }
    }
    
    public UnityEvent ChangedEvent = new();
    
    public bool AllOk => Oxygen >= 0.3f && !DustStorm;
}

public class Ship : MonoBehaviour
{
    private static readonly int Strength = Shader.PropertyToID("_Strength");
    [SerializeField] private float oxygenDepletionRate = 0.01f;
    [SerializeField] private float oxygenIncreaseRate = 0.01f;
    [SerializeField] DustStorm dustStorm;
    [SerializeField] private Material lowOxygenMat;
    [SerializeField] private float lowOxygenEffectStart = 0.5f;
    [SerializeField] private Volume volume;
    [SerializeField] private float maxVignette = 0.5f;

    private bool _ventsOpen = true;
    public bool VentsOpen
    {
        get => _ventsOpen;
        set
        {
            if(_ventsOpen == value) return;
            _ventsOpen = value;
            OnVentsOpenChange();
        }
    }

    [System.NonSerialized] public ShipStatus Status = new();
    private Vignette vignette;

    private void Start()
    {
        dustStorm.WarningEvent.AddListener(OnDustStormWarning);
        dustStorm.StartedEvent.AddListener(OnDustStormStarted);
        volume.profile.TryGet(out vignette);
    }
    
    private void OnDustStormStarted()
    {
        if (VentsOpen)
        {
            // die
        }
    }
    
    private void OnDustStormWarning()
    {
        Status.DustStorm = true;
    }
    
    private void OnDustStormEnded()
    {
        Status.DustStorm = false;
    }

    private void Update()
    {
        if (VentsOpen)
        {
            Status.Oxygen += oxygenDepletionRate * Time.deltaTime;
        }
        else
        {
            Status.Oxygen -= oxygenIncreaseRate * Time.deltaTime;
        }
        
        float t = 1 - Status.Oxygen / (1 - lowOxygenEffectStart);
        lowOxygenMat.SetFloat(Strength, Mathf.Lerp(0, 1, t));
        vignette.intensity.value = Mathf.Lerp(0, maxVignette, t);
    }
    
    private void OnVentsOpenChange()
    {
        if (!VentsOpen)
        {
            AudioManager.Instance.Play("LowOxygen");
        }
        else
        {
            AudioManager.Instance.Stop("LowOxygen");
        }
    }

    private void OnDestroy()
    {
        lowOxygenMat.SetFloat(Strength, 0);
        vignette.intensity.value = 0;
    }
}
