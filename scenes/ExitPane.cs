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
		EmitSignal(SignalName.QuitGame);
	}

	[Signal]
	public delegate void QuitGameEventHandler();
}
