using UnityEngine;

public class CameraCommand : ShellCommand
{
    protected override void OnExecute(string[] args)
    {
        Terminal.Println("Camera.", true);
    }
}

