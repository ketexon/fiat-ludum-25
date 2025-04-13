using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameBase : ShellCommand
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMPro.TMP_Text controlsText;
    [SerializeField] string controls = "Q: Exit";

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        controlsText.text = controls;
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
        Terminal.SubmitEvent.AddListener(OnSubmit);
    }

    protected virtual void OnMove(Vector2 dir)
    {
    }

    protected virtual void OnSubmit(){
    }

    void OnExit()
    {
        EndGame();
    }

    protected virtual void EndGame()
    {
        Terminal.MoveEvent.RemoveListener(OnMove);
        Terminal.ExitEvent.RemoveListener(OnExit);
        Terminal.SubmitEvent.RemoveListener(OnSubmit);

        IEnumerator WaitOneFrame(){
            yield return null;
            Terminal.TakingInput = true;
            Terminal.Visible = true;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }
        StartCoroutine(WaitOneFrame());
    }
}
