using UnityEngine;

public class CameraCommand : ShellCommand
{
    public override void Execute(string[] args)
    {
        base.Execute(args);
        
        Terminal.Println("Camera.", true);
    }
}

