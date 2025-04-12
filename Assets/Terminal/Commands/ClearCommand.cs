using UnityEngine;

public class ClearCommand : ShellCommand
{
    public override void Execute(string[] args)
    {
        base.Execute(args);
        
        if (args.Length != 1)
        {
            PrintHelp();
            return;
        }
        
        Terminal.Clear();
    }
}
