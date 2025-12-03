### USER
	Photo: exit_pane_script_error.png

	public override void _Ready()
	{
		rng.Randomize();

		player = GetNode<Player>("Player");
		battleUI = GetNode<BattleUI>("CanvasLayer/BattleUI");
		infoPane = GetNode<InfoPane>("CanvasLayer/InfoPane");
		exitPanel = GetNode<ExitPane>("CanvasLayer/ExitPane");

		var camera = GetNode<Camera2D>("Player/MainCamera");
		camera.LimitLeft = (int)SpawnMin.X;
		camera.LimitTop = (int)SpawnMin.Y;
		camera.LimitRight = (int)SpawnMax.X;
		camera.LimitBottom = (int)SpawnMax.Y;
		camera.LimitEnabled = true;

		camera.Position = player.Position;

		player.EnterBattle += StartBattle;
		infoPane.UpdateWords += RefreshInfoPane;
		infoPane.Hidden += () => { ChangeState(GameState.Hub); };
		battleUI.Hidden += () => { ChangeState(GameState.Hub); };
		exitPanel.QuitGame += SaveAndQuit;
		exitPanel.SaveGame += () => { Save(Globals.SAVE_FILE_PATH);};

using Godot;
using System;

public partial class ExitPane : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}

	void _on_exit_button_pressed()
	{
		GetTree().ChangeSceneToFile(@"res://scenes/main_menu.tscn");
	}

	void _on_save_button_pressed()
	{
		EmitSignal(SignalName.SaveGame);
	}

	[Signal]
	public delegate void QuitGameEventHandler();
	[Signal]
	public delegate void SaveGameHandler();
}

### Assistance
You're really close, but Godot C# has two strict rules for signals that are causing your errors:

✅ Fix Summary
1. Signal delegate names must end with EventHandler

Godot requires C# signal delegates to end with EventHandler.
Your SaveGameHandler does not end with EventHandler.

2. The signal name must match what you're calling in EmitSignal

You emit:

EmitSignal(SignalName.SaveGame);


So your signal must be named SaveGame, not SaveGameHandler.

✅ Corrected ExitPane.cs
using Godot;
using System;

public partial class ExitPane : PanelContainer
{
	[Signal]
	public delegate void QuitGameEventHandler();

	[Signal]
	public delegate void SaveGameEventHandler();

	public override void _Ready()
	{
	}

	private void _on_exit_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void _on_save_button_pressed()
	{
		EmitSignal(SignalName.SaveGame);   // now valid
	}
}

🎉 Why this fixes the errors
Error 1:

The name of the delegate 'ExitPane.SaveGameHandler' must end with 'EventHandler'

✔ Fixed by renaming:

public delegate void SaveGameEventHandler();

Error 2:

'ExitPane.SignalName' does not contain a definition for 'SaveGame'

This happens because Godot auto-generates:

SignalName.SaveGameEventHandler


But the actual signal name is everything BEFORE “EventHandler”:

SaveGame

So:

EmitSignal(SignalName.SaveGame);


now resolves correctly.

⚡ And now your HubWorld code works:
exitPanel.QuitGame += SaveAndQuit;
exitPanel.SaveGame += () => { Save(Globals.SAVE_FILE_PATH); };


If you want, I can also help you wire the buttons automatically in the Godot editor or through code.
