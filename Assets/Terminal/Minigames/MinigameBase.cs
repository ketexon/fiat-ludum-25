using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameBase : ShellCommand
{
    [SerializeField] CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    public override void Execute(string[] args)
    {
        base.Execute(args);

        Terminal.TakingInput = false;
        Terminal.Visible = false;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }

    protected virtual void StartGame()
    {
        Terminal.MoveEvent.AddListener(OnMove);
    }

    protected virtual void OnMove(Vector2 dir)
    {
    }

    protected virtual void EndGame()
    {
        Terminal.MoveEvent.RemoveListener(OnMove);
        
        Terminal.TakingInput = true;
        Terminal.Visible = true;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
