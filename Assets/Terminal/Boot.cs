using System.Collections;
using UnityEngine;

public class Boot : TerminalProgram
{
    [SerializeField, Multiline] private string bootText;
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
        
        var oldOutputInterval = Terminal.OutputInterval;
        Terminal.OutputInterval = 0.01f;

        
		AudioManager.Instance.Play("CompHum");
        Terminal.Println(bootText);

        Terminal.BufferPrintedEvent.AddListener(OnFinishPrinting);

        IEnumerator PostBoot(){
            yield return new WaitForSeconds(1.5f);
            Terminal.OutputInterval = oldOutputInterval;
            Terminal.BufferPrintedEvent.RemoveListener(OnFinishPrinting);
            Terminal.SwitchProgram<Login>();
        }

        void OnFinishPrinting()
        {
            StartCoroutine(PostBoot());
        }
    }
}