using Godot;
using System;
using System.Collections.Generic;

public partial class BattleUI : Control
{
    //Player refrence
    //[Export]
    //private Player player;
    //private List<Enemy> enemies = new();

    //public Player Player
    //{
    //    get { return player; }
    //    set { player = value; }
    //}
    //public List<Enemy> Enemies
    //{
    //    get { return enemies; }
    //    set { enemies = value; }
    //}

    // Enemy refrence

    private HealthBar enemyHealthBar;
    private HealthBar playerHealthBar;

    private LogPane enemyLogPane;
    private LogPane playerLogPane;

    private MovePane movePane; private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        enemyHealthBar = GetNode<HealthBar>("EnemyHealth");
        playerHealthBar = GetNode<HealthBar>("PlayerHealth");

        enemyLogPane = GetNode<LogPane>("EnemyLog");
        playerLogPane = GetNode<LogPane>("PlayerLog");

        movePane = GetNode<MovePane>(nameof(MovePane));

        //TODO: grab player refrence

        //player.UpdateHealth += UpdatePlayerHealth;
        //UpdatePlayerHealth();
    }

    public void UpdateMoves(Move[] moves)
    {

        movePane.ClearMoves();
        foreach (var move in moves)
        {
            movePane.AddMove(move);
        }
        //throw new NotImplementedException();
    }

    public void UpdatePlayerHealth(float healthPercent)
    {
        playerHealthBar.Value = healthPercent;
    }
    public void UpdateEnemyHealth(float healthPercent)
    {
        enemyHealthBar.Value = healthPercent;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}


    [Signal]
    public delegate void SelectPlayerMovesEventHandler(int count);
}
