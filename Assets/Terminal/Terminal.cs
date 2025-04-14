using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public struct TerminalState
{
    public string Username;
    public string RoverName;
}

public class Terminal : MonoBehaviour
{
    public TMP_Text text;

    [SerializeField] public ScreenLights ScreenLights;
    [SerializeField] private float blinkInterval = 0.5f;
    [FormerlySerializedAs("outputInterval"),SerializeField]
    public float OutputInterval = 0.01f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] TerminalProgram startProgram;

    [System.NonSerialized] public bool TakingInput = true;
    [System.NonSerialized] public bool Visible = true;
    [System.NonSerialized] public TerminalState State = new();

    [System.NonSerialized]
    public UnityEvent<Vector2> MoveEvent = new();

    [System.NonSerialized]
    public UnityEvent ExitEvent = new();

    [System.NonSerialized]
    public UnityEvent SubmitEvent = new();

    [System.NonSerialized]
    public UnityEvent ResetEvent = new();
    
    [System.NonSerialized]
    public UnityEvent BufferPrintedEvent = new();

    private bool InputPromptVisible => TakingInput && string.IsNullOrEmpty(bufferQueue);

    TerminalProgram program = null;

    private string buffer = "";
    private string bufferQueue = "";

    public string Input { get; private set; } = "";
    private int cursorPos = 0;
    private bool cursorVisible = true;

    private Coroutine blinkCoroutine = null;
    
	List<string> inputHistory = new();
	int historyIndex = -1;

    private string InputWithCursor {
        get {
            string inputMasked;
            if (cursorVisible)
            {
                inputMasked = (
                    Input[..cursorPos]
                    + "<mark color=#00ff00>f</mark>"
                );
                if (cursorPos+1 < Input.Length)
                {
                    inputMasked += Input[(cursorPos+1)..];
                }
            }
            else
            {
                inputMasked = Input;
            }
            return inputMasked;
        }
    }

    private string InputHidden => new('*', Input.Length);

    private string TransformedInput => program.InputHidden ? InputHidden : Input;


    private void OnEnable()
    {
        Keyboard.current.onTextInput += OnChar;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= OnChar;
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void Start()
    {
        program = startProgram;
        program.Terminal = this;
        program.enabled = true;

        StartCoroutine(BufferUpdateCoro());
        Blink();
    }

    private void Update()
    {
        Render();
    }

    private IEnumerator BufferUpdateCoro(){
        while (true)
        {
            while (bufferQueue.Length > 0)
            {
                char ch = bufferQueue[0];
                if(ch == '<'){
                    int next = bufferQueue.IndexOf('<', 1);
                    int end = bufferQueue.IndexOf('>');
                    if(end != -1 && (next == -1 || end < next)){
                        string tag = bufferQueue[..(end+1)];
                        buffer += tag;
                        bufferQueue = bufferQueue[(end+1)..];
                        continue;
                    }
                }
                if(char.IsWhiteSpace(ch)){
                    // skip all whitespace
                    while(true){
                        buffer += ch;
                        bufferQueue = bufferQueue[1..];
                        if(bufferQueue.Length == 0) break;
                        ch = bufferQueue[0];
                        if(!char.IsWhiteSpace(ch)) break;
                    }
                }
                else{
                    buffer += ch;
                    bufferQueue = bufferQueue[1..];
                }
                yield return new WaitForSeconds(OutputInterval);

                if (bufferQueue.Length == 0)
                {
                    BufferPrintedEvent.Invoke();
                }
            }
            yield return null;
        }
    }

    public void Println(string msg, bool force = false)
    {
        Print(msg + '\n', force);
    }

    public void Print(string msg, bool force = false){
        if(force){
            buffer += msg;
            return;
        }
        bufferQueue += msg;
    }

    public void Clear(){
        buffer = "";
        bufferQueue = "";
    }

	private void OnChar(char ch)
    {
        if (Pause.Paused) return;
        AudioManager.Instance.Play("KeyClick");
        if(!InputPromptVisible) return;
        switch (ch)
        {
            case var _ when !char.IsControl(ch):
                InsertChar(ch);
                return;
        }

        program.OnChar(ch);
    }

    private void InsertChar(char ch)
    {
        Input = Input[..cursorPos] + ch + Input[cursorPos..];
        cursorPos++;
        Blink();
        historyIndex = -1;
    }

    private void DeleteChar()
    {
        if (cursorPos == 0) return;
        Input = Input[..(cursorPos - 1)] + Input[cursorPos..];
        cursorPos--;
        Blink();
        historyIndex = -1;
    }

    public void OnUINavigate(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        var dir = Vector2Int.RoundToInt(ctx.ReadValue<Vector2>());
        if(dir.x != 0)
        {
            UpdateCursorPos(dir.x);
            Blink(true);
        }

        if (dir.y != 0)
        {
            ChangeHistory(dir.y);
            Blink(false);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        if (ctx.started) return;
        MoveEvent.Invoke(ctx.ReadValue<Vector2>());
    }

    public void OnExit(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        if (ctx.started)
        {
            ExitEvent.Invoke();
        }
    }

    public void OnInputCtrlC(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) return;
        if (Pause.Paused) return;
        if(!InputPromptVisible) return;
        
        Println(program.Prompt + TransformedInput + "^C", true);
        ClearInput();
    }

    public void OnInputCtrlL(InputAction.CallbackContext ctx)
    {
        Clear();
    }

    public void OnInputSubmit(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        if (!ctx.started)
        {
            return;
        }
        SubmitEvent.Invoke();

        if(!InputPromptVisible) return;
        Println(program.Prompt + TransformedInput, true);
        if (!program.InputHidden && !string.IsNullOrEmpty(Input))
        {
            inputHistory.Add(Input);
        }
        program.OnSubmit();
        ClearInput();
        historyIndex = -1;
    }
    
    public void OnInputReset(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        if (ctx.started)
        {
            ResetEvent.Invoke();
        }
    }

    public void OnInputBackspace(InputAction.CallbackContext ctx)
    {
        if (Pause.Paused) return;
        if (ctx.started)
        {
            DeleteChar();
        }
    }

    void UpdateCursorPos(int delta)
    {
        cursorPos += delta;
        if (cursorPos < 0)
        {
            cursorPos = 0;
        }
        else if (cursorPos > Input.Length)
        {
            cursorPos = Input.Length;
        }
    }

    public void SwitchProgram<T>() where T : TerminalProgram
    {
        if (program != null)
        {
            program.enabled = false;
        }
        program = GetComponentInChildren<T>();
        if (program == null)
        {
            Debug.LogError($"Program {typeof(T)} not found on {gameObject.name}");
        }
        program.Terminal = this;
        program.enabled = true;
        ClearInput();
    }

    void ChangeHistory(int delta)
    {
        Debug.Log(delta);
        
        delta *= -1;
        if (inputHistory.Count == 0) return;

        if (historyIndex == -1)
        {
            if (delta == -1)
            {
                historyIndex = inputHistory.Count - 1;
                Input = inputHistory[historyIndex];
                GoToEnd();
            }
        }
        else
        {
            historyIndex += delta;
            if (historyIndex >= inputHistory.Count)
            {
                historyIndex = -1;
                Input = "";
                GoToEnd();
            }
            else if(historyIndex < 0)
            {
                historyIndex = 0;
            }
            else
            {
                Input = inputHistory[historyIndex];
                GoToEnd();
            }
        }
    }

    private void ClearInput(){
        Input = "";
        cursorPos = 0;
    }

    private void GoToEnd()
    {
        cursorPos = Input.Length;
    }

    private void Render()
    {
        if (!Visible)
        {
            text.text = "";
            return;
        }
        text.text = buffer;
        if(InputPromptVisible){
            text.text += program.Prompt;
            if(program.InputHidden)
            {
                text.text += InputHidden;
            }
            else
            {
                text.text += InputWithCursor;
            }
        }
    }

    private void Blink(bool value = false)
    {
        cursorVisible = value;
        IEnumerator Coro()
        {
            yield return new WaitForSeconds(blinkInterval);
            blinkCoroutine = null;
            Blink(!cursorVisible);
        }
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(Coro());
    }
}
