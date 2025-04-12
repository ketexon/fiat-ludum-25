using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Terminal : MonoBehaviour
{
    public TMP_Text text;

    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private PlayerInput playerInput;
    
    private string buffer = "";
    private string input = "";
    private int cursorPos = 0;
    private bool cursorVisible = true;

    private string username;

    private Coroutine blinkCoroutine = null;

    private string Prompt => $"{username}@KetexOS:~$ ";
    
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
        Blink();
        Render();
    }

    private void Update()
    {
        Render();
        Debug.Log($"CursorPos: {cursorPos}");
    }

    public void Println(string msg)
    {
        buffer += msg + "\n";
    }

    private void OnChar(char ch)
    {
        Debug.Log((int) ch);
        
        if (ch is '\n' or '\r')
        {
            SubmitCommand();
            return;
        }

        if (ch is '\b')
        {
            DeleteChar();
            return;
        }

        if (!char.IsControl(ch))
        {
            InsertChar(ch);;
            return;
        }
    }

    private void InsertChar(char ch)
    {
        input = input[..cursorPos] + ch + input[cursorPos..];
        cursorPos++;
        Blink();
    }
    
    private void DeleteChar()
    {
        if (cursorPos == 0) return;
        input = input[..(cursorPos - 1)] + input[cursorPos..];
        cursorPos--;
        Blink();
    }

    public void OnUINavigate(InputAction.CallbackContext ctx)
    {
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

    void UpdateCursorPos(int delta)
    {
        cursorPos = cursorPos + delta;
        if (cursorPos < 0)
        {
            cursorPos = 0;
        }
        else if (cursorPos > input.Length)
        {
            cursorPos = input.Length;
        }
    }

    void ChangeHistory(int delta)
    {
        
    }

    private void SubmitCommand()
    {
        
    }

    private void Render()
    {
        string inputMasked;
        if (cursorVisible)
        {
            inputMasked = (
                input[..cursorPos]
                + "<mark color=#00ff00>f</mark>"
            );
            if (cursorPos+1 < input.Length)
            {
                inputMasked += input[(cursorPos+1)..];
            }
        }
        else
        {
            inputMasked = input;
        }

        text.text = buffer + Prompt + inputMasked;
    }

    private void Blink(bool value = false)
    {
        cursorVisible = value;
        IEnumerator Coro()
        {
            yield return new WaitForSeconds(blinkInterval);
            blinkCoroutine = null;
            Blink(!cursorVisible);
            Render();
        }
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(Coro());
    }
}
