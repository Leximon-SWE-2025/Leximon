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

    private void DisplayWordInfo(string word)
    {
        var infoContainer = GetNode<VBoxContainer>("HBoxContainer/ScrollContainer/InfoContainer");

        var wordLabel = infoContainer.GetNode<Label>("Word");
        var typeLabel = infoContainer.GetNode<Label>("Types");
        var definitionLabel = infoContainer.GetNode<Label>("Definitions");
        //if (wordLabel is null) {
        //    GD.Print("Stop yukking my yum");
        //}
        var synonymLabel = infoContainer.GetNode<Label>("Synonyms");
        var antonymLabel = infoContainer.GetNode<Label>("Antonyms");

        wordLabel.Text = WordManager.TitleCaseWord(word);
        typeLabel.Text = $"Types: {string.Join(", ", WordManager.GetTypes(word))}";
        definitionLabel.Text = $"Definitions: {string.Join("\n", WordManager.GetDefinitions(word))}";
        synonymLabel.Text = $"Synonyms: {string.Join(", ", WordManager.GetSynonyms(word))}";
        antonymLabel.Text = $"Antonyms: {string.Join(", ", WordManager.GetAntonyms(word))}";
    }

    public void AddWord(string word)
    {
        var button = new Button();
        //label.HorizontalAlignment = HorizontalAlignment.Center;
        //label.VerticalAlignment = VerticalAlignment.Center;
        button.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        button.SizeFlagsVertical = SizeFlags.Expand;

        button.Alignment = HorizontalAlignment.Center;
        button.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        button.AddThemeFontSizeOverride("font size", 30);
        //label.LabelSettings = new LabelSettings()
        //{
        //    FontSize = 30
        //};
        button.Pressed += () => DisplayWordInfo(word);
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
