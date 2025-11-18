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

    public string Text
    {
        get
        {
            return GetNode<Label>("VBoxContainer/Def").Text;

        }
        set
        {
            GetNode<Label>("VBoxContainer/Def").Text = value;
        }
    }
    public string Word
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

    void _on_button_pressed()
    {
        Close();
        //EmitSignalCloseRequested();
    }

    public void Close()
    {
        EmitSignal(SignalName.CloseDefinitionPopUp);
        Hide();
    }

    [Signal]
    public delegate void CloseDefinitionPopUpEventHandler();
}
