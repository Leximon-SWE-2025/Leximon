using Godot;
using System;
using System.Linq;

public partial class MovePane : Control
{
    [Export]
    PackedScene cardScene;

    private HBoxContainer moveContainer;

    //private Panel background;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        moveContainer = GetNode<HBoxContainer>("MoveContainer");
        //background = GetNode<Panel>("MoveBackground");

        //this.VisibilityChanged += UpdateCards;

    }



    public void ClearMoves()
    {
        foreach (var child in moveContainer.GetChildren())
        {
            if (child is Card)
            {
                moveContainer.RemoveChild(child);
            }
        }
    }

    public void AddMove(Move move)
    {
        var card = cardScene.Instantiate<Card>();
        card.SetLabel(move.Word);

        moveContainer.AddChild(card);

        //card.UpdatePosition();
    }
}
