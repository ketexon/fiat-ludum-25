using System.Collections;
using UnityEngine;

public class Boot : TerminalProgram
{
    [SerializeField] private float prebootTime = 2f;
    
    public override bool InputHidden => true;

    void OnEnable()
    {
        StartCoroutine(BootCoro());
    }

    IEnumerator BootCoro()
    {
        yield return new WaitForSeconds(prebootTime);
        Terminal.ScreenLights.Power = true;
        Terminal.SwitchProgram<Login>();
    }
}