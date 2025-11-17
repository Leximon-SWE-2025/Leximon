using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ;

[GlobalClass]
public abstract partial class Character : Area2D
{
    private int currentHealth=100;
    public int CurrentHealth => currentHealth;


    private int maxHealth=100;
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


    protected HashSet<Move> knownMoves = new();

    public List<Move> KnownMoves => [.. knownMoves];


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



        int[] indexes = [.. Enumerable.Range(0,knownMoves.Count)];
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

    [Signal]
    public delegate void UpdateHealthEventHandler();

    [Signal]
    public delegate void EnterBattleEventHandler(Character c);
}
