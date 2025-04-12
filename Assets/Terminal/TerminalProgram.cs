using UnityEngine;

public class TerminalProgram : MonoBehaviour {
	public virtual string Prompt => "";
	public virtual bool InputHidden => false;

	[System.NonSerialized] public Terminal Terminal;

	public virtual void OnChar(char ch)
	{}

	public virtual void OnSubmit()
	{}
}