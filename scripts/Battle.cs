using Godot;
using System;

public partial class Battle : Control
{
    // Player refrence
    // Enemy refrence

    private HealthBar enemyHealthBar;
    private HealthBar playerHealthBar;

    private LogPane enemyLogPane;
    private LogPane playerLogPane;

    private MovePane movePane;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        enemyHealthBar = GetNode<HealthBar>("EnemyHealth");
        playerHealthBar = GetNode<HealthBar>("PlayerHealth");

        enemyLogPane = GetNode<LogPane>("EnemyLog");
        playerLogPane = GetNode<LogPane>("PlayerLog");

        movePane = GetNode<MovePane>(nameof(MovePane));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
