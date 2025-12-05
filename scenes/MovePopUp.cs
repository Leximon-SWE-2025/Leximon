using Godot;
using System;

public partial class MovePopUp : PanelContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}
    public String Text
    {
        get
        {
            return GetNode<Label>("VBoxContainer/Word").Text;

        }
        set
        {
            GetNode<Label>("VBoxContainer/Word").Text = value;
        }
    }

    void _on_attack_button_pressed()
    {
        EmitSignal(SignalName.PlayerAttack, Text);
        Close();

    }

    void _on_defend_button_pressed()
    {
        EmitSignal(SignalName.PlayerDefend, Text);
        Close();
    }

    void _on_exit_button_pressed()
    {
        Close();
    }

    public void Close()
    {
        EmitSignal(SignalName.CloseMovePopUp);
        Hide();
    }

    [Signal]
    public delegate void CloseMovePopUpEventHandler();

    [Signal]
    public delegate void PlayerAttackEventHandler(string word);
    [Signal]
    public delegate void PlayerDefendEventHandler(string word);
}
