using UnityEngine;

public class VentsCommand : ShellCommand
{
    [SerializeField] private Ship ship;
    protected override void OnExecute(string[] args)
    {
        if (args.Length == 1)
        {
            Terminal.Println($"Vents: {(ship.VentsOpen ? "OPEN" : "CLOSED")}");
            return;
        }
        if (args.Length == 2)
        {
            if (args[1].ToLower() == "open")
            {
                ship.VentsOpen = true;
                Terminal.Println("Opening vents...");
                return;
            }

            if (args[1].ToLower() == "close")
            {
                ship.VentsOpen = false;
                Terminal.Println("Closing vents...");
                return;
            }
            
            Terminal.Println($"Unknown argument: {args[1]}");
        }
        
        PrintHelp();
    }
}
