using UnityEngine;

public class StatusCommand : ShellCommand
{
    public override void Execute(string[] args)
    {
        base.Execute(args);
        
        Terminal.Println("Status.", true);
    }
}
