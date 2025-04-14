using System.Collections.Generic;
using UnityEngine;

public class ShellCommand : MonoBehaviour
{
    [SerializeField] public string CommandName;
    [SerializeField] public List<string> CommandAliases;
    [SerializeField] public string CommandDescription;
    [SerializeField] public string CommandUsage;

    [System.NonSerialized] public Terminal Terminal;
    [System.NonSerialized] public Shell Shell;
    
    public void Execute(string[] args)
    {
        if(args.Length == 2 && args[1] == "help")
        {
            PrintHelp();
        }
        else
        {
            OnExecute(args);
        }
    }

    protected virtual void OnExecute(string[] args)
    {
        
    }

    protected void PrintHelp()
    {
        Shell.TryExecuteCommand($"help {CommandName}");
    }
}
