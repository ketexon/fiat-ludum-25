using UnityEngine;

public class TutorialCommand : ShellCommand
{
    protected override void OnExecute(string[] args)
    {
        Terminal.SwitchProgram<Tutorial>();
    }
}
