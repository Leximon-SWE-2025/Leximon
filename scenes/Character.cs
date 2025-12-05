using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
//using ;

public enum AttackStatus
{
    Effective,
    Normal,
    Ineffective
}

[GlobalClass]
public abstract partial class Character : Area2D, ISaveable
{
    //[Export]
    //Texture2D uiTexture;

    private double currentHealth = 100;
    public double CurrentHealth => currentHealth;

    //protected string defence;
    protected int baseDamage;
    protected double armor = 1.0;
    public double Armor => armor;

    protected string type;
    public string Type => type;

    private double maxHealth = 100;
    public double MaxHealth => maxHealth;

    public double PercentHealth
    {
        get
        {
            if (MaxHealth == 0)
            {
                return 0;
            }
            return CurrentHealth / MaxHealth * 100;
        }

        set => currentHealth = value switch
        {
            <= 0 or float.NaN => 0,
            > 0 and < 100 => (int)(MaxHealth * value / 100),
            >= 100 => MaxHealth,
        };
    }



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
    public Move SelectRandomMove() => knownMoves.RandomItem();
    public Move SelectRandomMove(Character effectiveAgainst, MoveType type)
    {
        var rand = Random.Shared.NextDouble();

        if (OS.IsDebugBuild())
        {
            GD.Print($"Effective move: rand = {rand}");
        }

        return type switch
        {
            MoveType.Attack => rand switch
            {
                > 0.5 => WordManager.GetTypeAntonyms(effectiveAgainst.Type).RandomItem(),

                > 0.2 => WordManager.GetTypeSynonyms(effectiveAgainst.Type).RandomItem(),

                _ => SelectRandomMove()
            },
            MoveType.Defend => rand switch
            {
                > 0.5 => WordManager.GetTypeSynonyms(effectiveAgainst.Type).RandomItem(),

                > 0.2 => WordManager.GetTypeAntonyms(effectiveAgainst.Type).RandomItem(),

                _ => SelectRandomMove()
            },
            _ => throw new NotImplementedException(),
        };


    }
    public MoveType SelectMoveType()
    {
        //(Random.Shared.NextDouble() * 100.0) > PercentHealth
        var rand = Random.Shared.NextDouble();
        if (OS.IsDebugBuild())
        {
            GD.Print($"rand: {rand}");
        }
        if (rand > 0.66)
        {
            return MoveType.Defend;
        }
        else
        {
            return MoveType.Attack;
        }
    }

    protected void RandomizeType() => type = WordManager.RandomTypes(1).First();

    public void SelectMoves(int count, Character target)
    {
        if (knownMoves.Count <= count)
        {
            currentMoves = [.. knownMoves];
            return;
        }
        //var newMoves = new HashSet<Move>(count);

        //this.GetNode<AnimatedSprite2D>("AnimatedSprite2D").SpriteFrames.GetFrameTexture("default", 0);
        var enemyType = target.Type;

        //var words = knownMoves.Random(5).ToArray();

        var enemyAntonyms = WordManager.GetTypeAntonyms(enemyType);
        var enemySynonyms = WordManager.GetTypeSynonyms(enemyType);

        var myAntonyms = WordManager.GetTypeAntonyms(Type);
        var mySynonyms = WordManager.GetTypeSynonyms(Type);


        var optimalMove = Random.Shared.NextDouble() switch
        {
            > 0.5 => enemyAntonyms.RandomItem(),
            _ => mySynonyms.RandomItem(),
        };
        var midMove = Random.Shared.NextDouble() switch
        {
            > 0.5 => enemySynonyms.RandomItem(),
            _ => myAntonyms.RandomItem(),
        };

        string[] setMoves = [optimalMove, midMove];

        var otherMoves = knownMoves.Random(count).Select(m => m.Word).Except(setMoves).Take(count - setMoves.Length);

        currentMoves = [.. otherMoves.Concat(setMoves)];


        Random.Shared.Shuffle(currentMoves);

        //int[] indexes = [.. Enumerable.Range(0, knownMoves.Count)];
        ////for (int i = 0; i < knownMoves.Count; i++)
        ////    indexes[i] = i;

        //Random rng = new Random();

        //// Shuffle only first n items
        //for (int i = 0; i < count; i++)
        //{
        //    int j = rng.Next(i, knownMoves.Count);
        //    (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
        //}


        //currentMoves = KnownMoves
        //    .Where((_, i) => indexes[..count]
        //    .Contains(i))
        //    .ToArray();
    }

    public void FullHeal() => currentHealth = maxHealth;

    public void ResetArmor() => armor = Globals.BaseDefence;

    public AttackStatus Attack(Move move, Character target)
    {
        var relation = WordManager.ClassifyRelation(move.Word, target.Type);

        if (OS.IsDebugBuild())
        {
            GD.Print($"{Name}: Relation: {relation}\tDamage: {baseDamage * relation.DamageMultiplier()}");
        }

        target.ApplyDamage(baseDamage * (float)relation.DamageMultiplier());

        return relation switch
        {
            Relation.None => AttackStatus.Ineffective,
            Relation.Synonym => AttackStatus.Normal,
            Relation.Antonym => AttackStatus.Effective,
            _ => throw new NotImplementedException(),
        };
    }

    public void Defend(Move move)
    {
        //defence = move;
        var relation = WordManager.ClassifyRelation(move, Type);

        armor += armor * (Globals.BaseDefenceIncrease * relation.DefenseMultiplier());

        //throw new NotImplementedException();
    }

    //public float EvaluateEffectiveness()
    //{
    //    throw new NotImplementedException();
    //}

    public void ApplyDamage(float damage)
    {
        if (damage <= 0) return;
        currentHealth -= (damage / Armor);
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
            //{nameof(knownMoves), knownMoves.Select(m=>m.Word).ToArray() },
            //{nameof(attack_multiplier), attack_multiplier},
            //{nameof(defense_multiplier), defense_multiplier},
            {nameof(type), type}
        };

        return data;
    }

    public virtual void Load(Dictionary<string, JsonElement> dict)
    {
        currentHealth = dict[nameof(currentHealth)].GetDouble();
        var positionData = dict[nameof(Position)].Deserialize<Dictionary<string, JsonElement>>();
        Position = new(positionData[nameof(Position.X)].GetSingle(), positionData[nameof(Position.Y)].GetSingle());
        maxHealth = dict[nameof(maxHealth)].GetDouble();
        //try
        //{
        //    var moves = dict[nameof(knownMoves)].Deserialize<string[]>();
        //    knownMoves = [.. moves.Select(m => new Move(m))];
        //}
        //catch (KeyNotFoundException)
        //{
        //    knownMoves = [];  // Enemies do not store this value, so this is fine
        //}
        knownMoves = [.. WordManager.Words]; // adds all words to the player (just for prototype)

        //attack_multiplier = dict[nameof(attack_multiplier)].GetSingle();
        //defense_multiplier = dict[nameof(defense_multiplier)].GetSingle();
        type = dict[nameof(type)].GetString();
    }

    [Signal]
    public delegate void UpdateHealthEventHandler();

    [Signal]
    public delegate void EnterBattleEventHandler(Character c);
}
