using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
//using ;

[GlobalClass]
public abstract partial class Character : Area2D, ISaveable
{
    //[Export]
    //Texture2D uiTexture;

    private int currentHealth = 100;
    public int CurrentHealth => currentHealth;


    private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    public float PercentHealth
    {
        get
        {
            if (MaxHealth == 0)
            {
                return 0;
            }
            return CurrentHealth / (float)MaxHealth * 100;
        }
    }

    public float attack_multiplier;
    public float defense_multiplier;


    protected HashSet<Move> knownMoves = [];

    public Move[] KnownMoves => [.. knownMoves];


    private Move[] currentMoves;

    public Move[] CurrentMoves => currentMoves;

    public bool IsAlive
    {
        get
        {
            return CurrentHealth > 0;
        }
    }

    public void SelectMoves(int count)
    {
        if (knownMoves.Count <= count)
        {
            currentMoves = knownMoves.ToArray();
            return;
        }
        //var newMoves = new HashSet<Move>(count);

        //this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").SpriteFrames.GetFrameTexture("default", 0);

        int[] indexes = [.. Enumerable.Range(0, knownMoves.Count)];
        //for (int i = 0; i < knownMoves.Count; i++)
        //    indexes[i] = i;

        Random rng = new Random();

        // Shuffle only first n items
        for (int i = 0; i < count; i++)
        {
            int j = rng.Next(i, knownMoves.Count);
            (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
        }


        currentMoves = KnownMoves
            .Where((_, i) => indexes[..count]
            .Contains(i))
            .ToArray();
    }

    public abstract void Attack();

    public abstract void Defend();

    public float EvaluateEffectiveness()
    {
        throw new NotImplementedException();
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= (int)damage;
        EmitSignal(SignalName.UpdateHealth);
    }

    public void Heal(float ammount)
    {
        currentHealth += (int)ammount;
        EmitSignal(SignalName.UpdateHealth);
    }

    public virtual Dictionary<string, object> Save()
    {
        var data = new Dictionary<string, object>()
        {
            {nameof(currentHealth), currentHealth},
            {nameof(Position), new Dictionary<string,object>(){
                                    {nameof(Position.X),Position.X },
                                    {nameof(Position.Y),Position.Y }
            }},
            {nameof(maxHealth),maxHealth },
            {nameof(knownMoves), knownMoves.Select(m=>m.Word).ToArray() },
            {nameof(attack_multiplier), attack_multiplier},
            {nameof(defense_multiplier), defense_multiplier}
        };

        return data;
    }

    public virtual void Load(Dictionary<string, JsonElement> dict)
    {
        currentHealth = dict[nameof(currentHealth)].GetInt32();
        var positionData = dict[nameof(Position)].Deserialize<Dictionary<string, JsonElement>>();
        Position = new(positionData[nameof(Position.X)].GetSingle(), positionData[nameof(Position.Y)].GetSingle());
        maxHealth = dict[nameof(maxHealth)].GetInt32();
        try
        {
            var moves = dict[nameof(knownMoves)].Deserialize<string[]>();
            knownMoves = [.. moves.Select(m => new Move(m))];
        }
        catch (KeyNotFoundException)
        {
            knownMoves = []; // Enemies do not store this value, so this is fine
        }
        attack_multiplier = dict[nameof(attack_multiplier)].GetSingle();
        defense_multiplier = dict[nameof(defense_multiplier)].GetSingle();
    }

    [Signal]
    public delegate void UpdateHealthEventHandler();

    [Signal]
    public delegate void EnterBattleEventHandler(Character c);
}
