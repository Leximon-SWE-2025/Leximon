using Godot;
using System;

public partial class Enemy : Character
{

    AnimatedSprite2D icon;
    private Random rand = new Random();
    public Move selectRandomMove()
    {
        // random number from 0 to 1
        // 0 represents Attack, 1 represents Defend
        int enemy_length = CurrentMoves.Length;
        int move_no = (rand.Next(0, enemy_length));
        return CurrentMoves[move_no];
    }
    public override void Attack(Move move)
    {
        throw new NotImplementedException();
    }

    public override void Defend(Move move)
    {
        throw new NotImplementedException();
    }

    public override void _Ready()
    {
        icon = GetNode<AnimatedSprite2D>("Icon");
        //var interactionArea = GetNode<Area2D>("InteractionArea");
        var interactionArea = GetNode<Area2D>("InteractionArea");

        interactionArea.AreaEntered += InteractionAreaEntered;
        interactionArea.AreaExited += InteractionAreaExited;
    }

    private void InteractionAreaEntered(Area2D body)
    {
        if (body is Player)
        {
            icon.Show();
        }
    }
    private void InteractionAreaExited(Area2D body)
    {
        if (body is Player)
        {
            icon.Hide();
        }
    }
}
