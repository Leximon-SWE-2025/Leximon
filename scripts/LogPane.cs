using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;





public record MoveLog(string Word, MoveType Type)
{
    public override string ToString() => $"{Type}: {Word}";
}

public partial class LogPane : Control
{

    private VBoxContainer messageContainer;
    private RichTextLabel messageLabel;
    private Label logLabel;
    private Queue<object> log = new();

    public string Label
    {
        get => logLabel.Text;
        set => logLabel.Text = value;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        messageContainer = GetNode<VBoxContainer>("Body/Affects");
        messageLabel = GetNode<RichTextLabel>("Body/Label");
        logLabel = GetNode<Label>("Body/LogLabel");
    }

    public void LogMove(string move, MoveType type) => Log(new MoveLog(move, type));


    public void Log<T>(T item)
    {
        log.Enqueue(item);
        //while (log.Count > 8)
        //{
        //    log.Dequeue();
        //}
    }

    public void Clear()
    {
        log.Clear();
    }

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Visible)
        {
            messageLabel.Text = string.Join("\n", log.Reverse());

        }
    }
}
