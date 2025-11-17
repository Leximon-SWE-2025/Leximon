using Godot;
using System;

public partial class Enemy : Character
{

    AnimatedSprite2D icon;
    // public Move selectRandomMove(); 
    public override void Attack()
    {
        throw new NotImplementedException();
    }

    public override void Defend()
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
