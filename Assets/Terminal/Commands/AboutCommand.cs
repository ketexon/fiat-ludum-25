using UnityEngine;

public class AboutCommand : ShellCommand
{
    public override void Execute(string[] args)
    {
        base.Execute(args);
        
        Terminal.Println("KetexOS v4.12.25\n\n(c) 2125 ReNova Laboratories ReNova Laboratories is leading the next generation of humanity into space.\n\nPlanet Promethos is our future. Your task is to locate, collect, and return high quantities of Resource X back to Earth.", true);
    }
}
