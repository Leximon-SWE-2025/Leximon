using Godot;
using System;

public partial class DefinitionPopUp : PanelContainer
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
            return GetNode<Label>("VBoxContainer/Label").Text;

        }
        set
        {
            GetNode<Label>("VBoxContainer/Label").Text = value;
        }
    }

    void _on_button_pressed()
    {
        EmitSignal(SignalName.CloseDefinitionPopUp);
        //EmitSignalCloseRequested();
    }

    [Signal]
    public delegate void CloseDefinitionPopUpEventHandler();
}
