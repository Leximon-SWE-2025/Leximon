using Godot;
using System;
using System.Linq;

public partial class InfoPane : PanelContainer
{
    private GridContainer wordContainer;

    private Label playerType;
    private Label stats;

    [Export] Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.VisibilityChanged += Display;

        wordContainer = GetNode<GridContainer>("VBoxContainer/HBoxContainer/ScrollContainer2/WordsContainer");

        playerType = GetNode<Label>("VBoxContainer/HBoxContainer2/PlayerType");
        stats = GetNode<Label>("VBoxContainer/HBoxContainer2/Stats");

        //wordContainer = GetNode<GridContainer>("HBoxContainer/WordsContainer");
    }
    private void Display()
    {
        if (Visible)
        {
            EmitSignal(SignalName.UpdateWords);
            UpdatePlayerInfo();
        }
    }

    private void UpdatePlayerInfo()
    {
        playerType.Text = $"Current type: {player.Type}";
        stats.Text = $"{player.CurrentHealth}/{player.MaxHealth} hp";
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
        var infoContainer = GetNode<VBoxContainer>("VBoxContainer/HBoxContainer/ScrollContainer/InfoContainer");

        var wordLabel = infoContainer.GetNode<Label>("Word");
        var typeLabel = infoContainer.GetNode<Label>("Types");
        var definitionLabel = infoContainer.GetNode<Label>("Definitions");
        //if (wordLabel is null) {
        //    GD.Print("Stop yukking my yum");
        //}
        var synonymLabel = infoContainer.GetNode<Label>("Synonyms");
        var antonymLabel = infoContainer.GetNode<Label>("Antonyms");

        wordLabel.Text = WordManager.TitleCaseWord(word);
        //typeLabel.Text = $"Types: {string.Join(", ", WordManager.GetTypes(word))}";
        definitionLabel.Text = $"Definitions: {string.Join("\n", WordManager.GetDefinitions(word).Select(def => $"- {def}"))}";
        synonymLabel.Text = $"Synonym Types: {string.Join(", ", WordManager.GetSynonyms(word))}";
        antonymLabel.Text = $"Antonym Types: {string.Join(", ", WordManager.GetAntonyms(word))}";
    }

    public void AddWord(string word)
    {
        var button = new Button
        {
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.Expand,

            Alignment = HorizontalAlignment.Center,
            AutowrapMode = TextServer.AutowrapMode.WordSmart
        };
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
