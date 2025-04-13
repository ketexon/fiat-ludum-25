using System.Collections.Generic;
using UnityEngine;

public class Tutorial : TerminalProgram
{
	[SerializeField] private List<string> tutorialText = new();
	private int curTutorialText = 0;

	public override string Prompt => "Press enter to continue. ";

	[SerializeField] private string osText = @"<size=50%>____      _   _                   _          _
|  _ \ ___| \ | | _____   ____ _  | |    __ _| |__  ___
| |_) / _ \  \| |/ _ \ \ / / _` | | |   / _` | '_ \/ __|
|  _ <  __/ |\  | (_) \ V / (_| | | |__| (_| | |_) \__ \
|_| \_\___|_| \_|\___/ \_/ \__,_| |_____\__,_|_.__/|___</size>";

	void OnEnable()
	{
		Terminal.TakingInput = true;
		Terminal.Println(osText);
		PrintNextText();
	}
	
	void PrintNextText()
	{
		Terminal.Println(tutorialText[curTutorialText]);
		curTutorialText++;
	}

	public override void OnSubmit()
	{
		base.OnSubmit();

		if (curTutorialText < tutorialText.Count)
		{
			PrintNextText();
		}
		else
		{
			Terminal.SwitchProgram<Shell>();
		}
	}
}