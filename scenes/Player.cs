using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;



public partial class Player : Character
{

    private Area2D interactionArea;
    private AnimatedSprite2D sprite;

    private Vector2 screenSize;
    public bool CanMove = true;

    private int battlesWon = 0;
    public int BattlesWon
    {
        get => battlesWon;

        set
        {
            battlesWon=value;
            if (battlesWon % Globals.WinsToSwitchType == 0)
            {
                RandomizeType();
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;

        RandomizeType();

        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        interactionArea = GetNode<Area2D>("InteractionArea");

        knownMoves = [.. WordManager.RandomMoves()];

        baseDamage = Globals.BasePlayerDamage;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        if (CanMove)
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
        sprite.FlipH = direction.X switch
        {
            < 0 => true,
            > 0 => false,
            _ => sprite.FlipH
        };
    }

    public void Move(Vector2 ammount)
    {
        if (ammount == Vector2.Zero) return;
        var new_pos = Position + ammount;
        UpdateSprite(ammount);

        if (new_pos.X < 0 || new_pos.X > screenSize.X || new_pos.Y < 0 || new_pos.Y > screenSize.Y) return;


        var tween = CreateTween();
        tween.Finished += () => CanMove = true;

        tween.TweenProperty(this, "position", Position + ammount, Globals.MovementTimeSec).SetTrans(Tween.TransitionType.Linear);

        CanMove = false;
    }

    public void add_move()
    {
        throw new NotImplementedException();
    }

   

    public override void Load(Dictionary<string, JsonElement> dict)
    {
        base.Load(dict);

        var words = WordManager.AllMoves;

        knownMoves = [.. knownMoves.Intersect(words)];
    }

    // public List<Moves> getMoveList();
}
