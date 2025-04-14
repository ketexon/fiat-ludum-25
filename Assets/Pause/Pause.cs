using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider slider;
    
    public static bool Paused = false;

    private GameObject lastSelected = null;

    private void Awake()
    {
        slider.onValueChanged.AddListener(SliderChanged);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;

        SliderChanged(slider.value);
    }

    public void OnInputPause(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) return;
        Paused = !Paused;
        
        Time.timeScale = Paused ? 0 : 1;
        AudioListener.pause = Paused;
        canvasGroup.alpha = Paused ? 1 : 0;
        canvasGroup.interactable = Paused;

        if (Paused)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            slider.Select();
            EventSystem.current.SetSelectedGameObject(slider.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
    
    void SliderChanged(float v)
    {
        audioMixer.SetFloat("MyExposedParam 2", Mathf.Lerp(-40, 0, v));
    }
}
