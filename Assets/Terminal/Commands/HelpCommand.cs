using UnityEngine;

public class HelpCommand : ShellCommand
{
    public override void Execute(string[] args)
    {
        base.Execute(args);
        
        if (args.Length == 1)
        {
            PrintCommands();
            return;
        }
        if (args.Length == 2)
        {
            PrintCommand(args[1]);
            return;
        }
        Terminal.Println("Invalid arguments.");
        Terminal.Println("Usage: help [command]");
    }

    void PrintCommands()
    {
        foreach(var (name, cmd) in Shell.Commands)
        {
            Terminal.Println($"{name} - {cmd.CommandDescription}");
        }
    }

    void PrintCommand(string name)
    {
        if (!Shell.Commands.TryGetValue(name, out var cmd))
        {
            Terminal.Println($"Command not found: {name}");
            Terminal.Println("Type `help` to see a list of available commands.");
            return;
        }
        Terminal.Println($"{name} - {cmd.CommandDescription}");
        Terminal.Println($"Usage: {cmd.CommandUsage}");
    }
}
