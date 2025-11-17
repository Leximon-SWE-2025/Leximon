using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready() {
		GD.Print("MainMenu Ready!");
		var b1 = GetNode<Button>("VBoxContainer/Button");
		var b2 = GetNode<Button>("VBoxContainer/Button2");
		var b3 = GetNode<Button>("VBoxContainer/Button3");
		var b4 = GetNode<Button>("VBoxContainer/Button4");
	}
	private void _on_start_pressed(){
		//GD.Print("Start Button Works");
		GetTree().ChangeSceneToFile(@"res://scenes/game.tscn");
	}
	
	private void _on_load_pressed(){
		GD.Print("Load Button Works");
	}
	
	private void _on_about_pressed(){
		//GD.Print("About Button Works");
		OS.ShellOpen(@"https://leximon-swe-2025.github.io/Website/");
	}
	
	private void _on_exit_pressed(){
		GetTree().Quit();
	}
}
