public class Shell : TerminalProgram
{
	public override string Prompt => ">";
	public override bool InputHidden => false;

	void OnEnable()
	{

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