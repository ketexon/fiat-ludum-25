using UnityEngine;

public class ClearCommand : ShellCommand
{
    protected override void OnExecute(string[] args)
    {
        if (args.Length != 1)
        {
            PrintHelp();
            return;
        }
        
        Terminal.Clear();
    }
}
