using Godot;
using System;

public partial class InfoPane : PanelContainer
{
    private GridContainer wordContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.VisibilityChanged += Display;

        wordContainer = GetNode<GridContainer>("HBoxContainer/ScrollContainer2/WordsContainer");
        //wordContainer = GetNode<GridContainer>("HBoxContainer/WordsContainer");
    }
    private void Display()
    {
        if (Visible)
        {
            EmitSignal(SignalName.UpdateWords);
        }
    }

    public void ClearWords()
    {
        foreach (var child in wordContainer.GetChildren())
        {
            wordContainer.RemoveChild(child);
        }
    }

    public void AddWord(string word)
    {
        var button = new Button();
        //label.HorizontalAlignment = HorizontalAlignment.Center;
        //label.VerticalAlignment = VerticalAlignment.Center;
        button.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        button.SizeFlagsVertical = SizeFlags.Expand;

        button.Alignment=HorizontalAlignment.Center;
        button.AutowrapMode= TextServer.AutowrapMode.WordSmart;
        button.AddThemeFontSizeOverride("font size", 30);
        //label.LabelSettings = new LabelSettings()
        //{
        //    FontSize = 30
        //};
        button.Text = word;
        wordContainer.AddChild(button);
    }

    [Signal]
    public delegate void UpdateWordsEventHandler();


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}
}
