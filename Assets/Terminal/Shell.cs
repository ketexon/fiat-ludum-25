using System;
using System.Collections.Generic;
using UnityEngine;

public class Shell : TerminalProgram
{
	[SerializeField] private string osName = "KetexOS";
	[SerializeField] DustStorm dustStorm;
	
	public override string Prompt => $"{Terminal.State.Username}@{osName}:~$ ";
	public override bool InputHidden => false;
	public IDictionary<string, ShellCommand> Commands = new SortedDictionary<string, ShellCommand>();
	public IDictionary<string, string> Aliases = new Dictionary<string, string>();

	public void TryExecuteCommand(string[] args)
	{
		if (args.Length == 0) return;
		
		var cmd = args[0];
		
		if(Aliases.TryGetValue(cmd, out var resolvedCmd)){
			cmd = resolvedCmd;
		}

		if (Commands.TryGetValue(cmd, out var command))
		{
			command.Execute(args);
		}
		else
		{
			Terminal.Println($"Command not found: {cmd}", true);
			Terminal.Println("Type `help` to see a list of available commands.", true);
		}
	}

	public void TryExecuteCommand(string argsString)
	{
		var args = ParseArgs(argsString);
		TryExecuteCommand(args);
	}
	
	void OnEnable()
	{
		dustStorm.enabled = true;
		Terminal.TakingInput = true;
		
		var commandList = GetComponentsInChildren<ShellCommand>();
		foreach (var command in commandList)
		{
			command.Terminal = Terminal;
			command.Shell = this;
			Commands.Add(command.CommandName, command);
			foreach(var alias in command.CommandAliases){
				Aliases.Add(alias, command.CommandName);
			}
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