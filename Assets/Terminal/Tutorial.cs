using UnityEngine;

public class Tutorial : TerminalProgram
{
	enum State {
	}

	[SerializeField] private string osText = @"<size=50%>____      _   _                   _          _
|  _ \ ___| \ | | _____   ____ _  | |    __ _| |__  ___
| |_) / _ \  \| |/ _ \ \ / / _` | | |   / _` | '_ \/ __|
|  _ <  __/ |\  | (_) \ V / (_| | | |__| (_| | |_) \__ \
|_| \_\___|_| \_|\___/ \_/ \__,_| |_____\__,_|_.__/|___</size>";

	void OnEnable()
	{
		Terminal.TakingInput = true;
		Terminal.Println(osText);
	}

	void OnDisable()
	{

	}

	public override void OnSubmit()
	{
		base.OnSubmit();

		if (string.IsNullOrEmpty(Terminal.Input))
		{
			return;
		}
	}
}