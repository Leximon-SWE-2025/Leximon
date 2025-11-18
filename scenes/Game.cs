using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
    Player Player;

    Enemy Enemy;

    BattleUI BattleUI;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var camera = GetNode<Camera2D>("Player/MainCamera");

        camera.LimitTop = 0;
        camera.LimitLeft = 0;

        camera.LimitRight = (int)GetViewportRect().Size.X;
        camera.LimitBottom = (int)GetViewportRect().Size.Y;

        camera.LimitEnabled = true;
        BattleUI = GetNode<BattleUI>("CanvasLayer/BattleUI");

        Player = GetNode<Player>("Player");
        Enemy = GetNode<Enemy>("Enemy");

        //var battleUI = GetNode<Battle>("BattleUI");

        //BattleUI.Player = Player;
        //BattleUI.Enemies.Add(Enemy);

        //Player.NotificationReady += () => GD.Print("Ready");
        //GD.Print($"Before: {Player.Position}");
        Player.Position = Player.Position.Snapped(Globals.TILE_SIZE) + (Vector2.One * (Globals.TILE_SIZE / 2));
        //GD.Print($"After: {Player.Position}");

        Enemy.Position = Enemy.Position.Snapped(Globals.TILE_SIZE) + (Vector2.One * (12 * Globals.TILE_SIZE / 2));

        Player.EnterBattle += StartBattle;
    }

    void StartBattle(Character enemy)
    {

        Player.SelectMoves(5);

        BattleUI.UpdateMoves(Player.CurrentMoves);
        BattleUI.UpdateEnemyHealth(100f);
        BattleUI.UpdatePlayerHealth(Player.PercentHealth);

        BattleUI.Show();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            BattleUI.Close();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /*	public override void _Process(double delta)
        {
        } */
}
