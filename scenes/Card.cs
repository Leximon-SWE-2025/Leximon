using Godot;
using System;

public partial class Card : Control
{
    [Export]
    float hoverAmmount;

    //private Vector2? hoverPosition;
    //private Vector2? groundedPosition;


    private Tween hoverTween;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}

    //public void UpdatePosition()
    //{
    //    hoverPosition = Position + Vector2.Up * hoverAmmount;
    //    groundedPosition = Position;
    //}


    public void HoverBy(Vector2 ammount)
    {
        hoverTween?.CustomStep(Double.MaxValue);
        hoverTween?.Kill();


        hoverTween = CreateTween();

        hoverTween.TweenProperty(this, "position", Position + ammount, Globals.MovementTimeSec).SetTrans(Tween.TransitionType.Linear);


    }

    void _on_mouse_entered() => HoverBy(Vector2.Up * hoverAmmount);

    void _on_mouse_exited() => HoverBy(Vector2.Down * hoverAmmount);

    void _on_gui_input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.IsReleased())
                GD.Print($"Mouse clicked for {GetNode<Label>("VBoxContainer/WordName").Text}");
        }
    }

    public void SetLabel(String text)
    {
        var WordLabel = GetNode<Label>("VBoxContainer/WordName");
        WordLabel.Text = text;
    }
    //public Card(string Word) : base()
    //{
    //    //var WordLabel = GetNode<Label>("VBoxContainer/WordName");
    //    //WordLabel.Text = Word;
    //    label = Word;
    //}
    //public Card(Move move) : this(move.Word) { }
}
