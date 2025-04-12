using UnityEngine;

public class MinigameBase : ShellCommand
{
    [SerializeField] CanvasGroup canvasGroup;
    
    public override void Execute(string[] args)
    {
        base.Execute(args);

        Terminal.TakingInput = false;
        canvasGroup.alpha = 1;
    }

    protected virtual void StartGame()
    {
        
    }

    protected void ExitGame()
    {
        Terminal.TakingInput = true;
        canvasGroup.alpha = 0;
    }
}
