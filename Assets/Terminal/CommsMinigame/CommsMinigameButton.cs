using System;
using UnityEngine;
using UnityEngine.UI;

public class CommsMinigameButton : MonoBehaviour
{
    private static readonly int RightProperty = Animator.StringToHash("right");
    private static readonly int ActiveProperty = Animator.StringToHash("active");

    public enum ButtonState
    {
        Inactive,
        Left,
        Right
    }
    
    [SerializeField] private Button button;
    [SerializeField] private Animator animator;

    [System.NonSerialized] public CommsMinigameLevel level;

    [System.NonSerialized] public ButtonState State = ButtonState.Inactive;

    private void Awake()
    {
        UpdateAnimator();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClick);
    }
    
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        switch (State)
        {
            case ButtonState.Inactive:
                State = ButtonState.Left;
                break;
            case ButtonState.Left:
                State = ButtonState.Right;
                break;
            case ButtonState.Right:
                State = ButtonState.Inactive;
                break;
        }

        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        animator.SetBool(ActiveProperty, State != ButtonState.Inactive);
        animator.SetBool(RightProperty, State == ButtonState.Right);
    }
}