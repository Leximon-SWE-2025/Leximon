using Godot;
using System;

public partial class ControlInfo : Control
{
    [Export]
    private string Text;

    [Export]
    private AtlasTexture Texture;

    //private Label label;

    //private TextureRect panel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var label = GetNode<Label>("HBoxContainer/Label");
        var panel = GetNode<TextureRect>("HBoxContainer/Panel");

        label.Text = Text;
        panel.Texture = Texture;
    }


}
