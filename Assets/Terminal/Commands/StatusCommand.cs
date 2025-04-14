using UnityEngine;

public class StatusCommand : ShellCommand
{
    protected override void OnExecute(string[] args)
    {
        Terminal.Println("Status.", true);
    }
}
