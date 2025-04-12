using System;
using System.Collections.Generic;
using UnityEngine;

public class Shell : TerminalProgram
{
	[SerializeField] private string osText = @"<size=50%>____      _   _                   _          _
|  _ \ ___| \ | | _____   ____ _  | |    __ _| |__  ___
| |_) / _ \  \| |/ _ \ \ / / _` | | |   / _` | '_ \/ __|
|  _ <  __/ |\  | (_) \ V / (_| | | |__| (_| | |_) \__ \
|_| \_\___|_| \_|\___/ \_/ \__,_| |_____\__,_|_.__/|___</size>";
	[SerializeField] private string osName = "KetexOS";
	
	public override string Prompt => $"{Terminal.State.Username}@{osName}:~$ ";
	public override bool InputHidden => false;
	public IDictionary<string, ShellCommand> Commands = new SortedDictionary<string, ShellCommand>();

	public void TryExecuteCommand(string[] args)
	{
		if (args.Length == 0) return;
		
		var cmd = args[0];
		
		if (Commands.TryGetValue(cmd, out var command))
		{
			command.Execute(args);
		}
		else
		{
			Terminal.Println($"Command not found: {cmd}");
			Terminal.Println("Type `help` to see a list of available commands.");
		}
	}

	public void TryExecuteCommand(string argsString)
	{
		var args = ParseArgs(argsString);
		TryExecuteCommand(args);
	}
	
	void OnEnable()
	{
		Terminal.TakingInput = true;
		Terminal.Println(osText);
		
		var commandList = GetComponentsInChildren<ShellCommand>();
		foreach (var command in commandList)
		{
			command.Terminal = Terminal;
			command.Shell = this;
			Commands.Add(command.CommandName, command);
		}
	}

	void OnDisable()
	{
		Commands.Clear();
	}

	public override void OnSubmit()
	{
		base.OnSubmit();

		if (string.IsNullOrEmpty(Terminal.Input))
		{
			return;
		}
		TryExecuteCommand(Terminal.Input);
	}

	static string[] ParseArgs(string input)
	{
		return input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
	}
}