using System.Collections;
using UnityEngine;

public class Login : TerminalProgram {

	[SerializeField, Multiline] string startMessage;
	enum State {
		Username,
		Password,
		Security,
		Starting,
	}

	State state = State.Username;
	public override string Prompt => state switch {
		State.Username => "Username: ",
		State.Password => "Password: ",
		State.Security => "",
		_ => "",
	};

	public override bool InputHidden => state is State.Password;

	public override void OnSubmit()
	{
		base.OnSubmit();

		if(string.IsNullOrEmpty(Terminal.Input))
		{
			Terminal.Println("Please enter a value.");
			return;
		}

		switch (state)
		{
			case State.Username:
				Terminal.State.Username = Terminal.Input;
				state = State.Password;
				break;

			case State.Password:
				state = State.Security;
				Terminal.Println("\nPlease answer the following security question. What is the rover's name?");
				break;

			case State.Security:
				Terminal.State.RoverName = Terminal.Input;
				Terminal.Println("");
				state = State.Starting;
				StartCoroutine(StartShellCoro());
				break;
		}
	}

	IEnumerator StartShellCoro(){
		Terminal.TakingInput = false;
		Terminal.Print("Starting");
		for (int i = 0; i < 3; i++)
		{
			Terminal.Print(".");
			yield return new WaitForSeconds(0.3f);
		}

		Terminal.Clear();
		Terminal.SwitchProgram<Tutorial>();
	}

	void OnEnable()
	{
		Terminal.Clear();
		Terminal.Println(startMessage);
	}
}