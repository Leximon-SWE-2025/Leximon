using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;


public partial class Player : Character
{

    private Area2D interactionArea;
    private AnimatedSprite2D sprite;

    private Vector2 screenSize;
    private bool canMove = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;


        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        interactionArea = GetNode<Area2D>("InteractionArea");

        knownMoves = new HashSet<Move>   {
            new Move(Word:"Cold"),
            new Move(Word:"Hot"),
            new Move(Word:"Wet"),
            new Move(Word:"Dry"),
            new Move(Word:"Bright"),
            new Move(Word:"Dark"),
            new Move(Word:"Fun"),
            new Move(Word:"Boring"),
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        if (canMove)
        {
            Move(direction * Globals.TILE_SIZE);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("enter_battle"))
        {
            TryEnterBattle();
        }
    }

    private bool TryEnterBattle()
    {
        Enemy[] enemies_in_range = interactionArea.GetOverlappingAreas().OfType<Enemy>().ToArray();

        Enemy enemy_to_fight;
        if (enemies_in_range.Length == 0)
        {
            return false;
        }
        else if (enemies_in_range.Length == 1)
        {
            enemy_to_fight = enemies_in_range[0];
        }
        else
        {
            enemy_to_fight = enemies_in_range.MinBy(e => Position - e.Position);
        }
        EmitSignal(SignalName.EnterBattle, enemy_to_fight);
        return true;
    }

    public void UpdateSprite(Vector2 moveDirection)

    {
        var direction = moveDirection.Normalized();
        if (direction == Vector2.Left)
        {
            sprite.FlipH = true;
        }
        else if (direction == Vector2.Right)
        {
            sprite.FlipH = false;
        }
    }

    public void Move(Vector2 ammount)
    {
        if (ammount == Vector2.Zero) return;
        var new_pos = Position + ammount;
        UpdateSprite(ammount);

        if (new_pos.X < 0 || new_pos.X > screenSize.X || new_pos.Y < 0 || new_pos.Y > screenSize.Y) return;


        var tween = CreateTween();
        tween.Finished += () => canMove = true;

        tween.TweenProperty(this, "position", Position + ammount, Globals.MovementTimeSec).SetTrans(Tween.TransitionType.Linear);

        canMove = false;
    }

    public void add_move()
    {
        throw new NotImplementedException();
    }

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    public override void Defend()
    {
        throw new NotImplementedException();
    }

    // public List<Moves> getMoveList();
}
