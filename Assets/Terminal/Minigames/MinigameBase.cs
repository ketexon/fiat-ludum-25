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
        
        StartGame();
    }

    protected virtual void StartGame()
    {
        Terminal.MoveEvent.AddListener(OnMove);
        Terminal.ExitEvent.AddListener(OnExit);
    }

    protected virtual void OnMove(Vector2 dir)
    {
    }

    void OnExit()
    {
        EndGame();
    }

    protected virtual void EndGame()
    {
        Terminal.MoveEvent.RemoveListener(OnMove);
        Terminal.ExitEvent.RemoveListener(OnExit);
        
        Terminal.TakingInput = true;
        Terminal.Visible = true;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
