using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

enum GameState
{
    Hub, Battle, Info
}

public partial class Game : Node2D
{
    Player Player;

    Enemy Enemy;

    BattleUI BattleUI;

    InfoPane InfoPane;

    GameState State;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        State = GameState.Hub;
        var camera = GetNode<Camera2D>("Player/MainCamera");

        camera.LimitTop = 0;
        camera.LimitLeft = 0;

        camera.LimitRight = (int)GetViewportRect().Size.X;
        camera.LimitBottom = (int)GetViewportRect().Size.Y;

        camera.LimitEnabled = true;
        BattleUI = GetNode<BattleUI>("CanvasLayer/BattleUI");

        Player = GetNode<Player>("Player");
        Enemy = GetNode<Enemy>("Enemy");

        InfoPane = GetNode<InfoPane>("CanvasLayer/InfoPane");
        //var battleUI = GetNode<Battle>("BattleUI");

        InfoPane.UpdateWords += () =>
        {
            InfoPane.ClearWords();
            foreach (var move in Player.KnownMoves)
            {
                InfoPane.AddWord(move.Word);

                if (OS.IsDebugBuild())
                {
                    //foreach (var i in Enumerable.Range(0, 20))
                    //{
                    //    InfoPane.AddWord($"dbg: {move.Word} {i}");
                    //}
                    GD.Print($"Adding {move.Word} to info pane");
                }
            }
        };

        InfoPane.Hidden += () => { State = GameState.Hub; };

        BattleUI.Hidden += () => { State = GameState.Hub; };
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
        if (State == GameState.Hub)
        {
            Player.SelectMoves(5);

            BattleUI.UpdateMoves(Player.CurrentMoves);
            BattleUI.UpdateEnemyHealth(100f);
            BattleUI.UpdatePlayerHealth(Player.PercentHealth);

            BattleUI.Show();
            State = GameState.Battle;
        }
    }

    private void OpenInfoPane()
    {
        InfoPane.Show();
        State = GameState.Info;
    }




    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            switch (State)
            {
                case GameState.Battle:
                    BattleUI.Close();
                    break;
                case GameState.Info:
                    InfoPane.Hide();
                    break;
                default: break;
            }
        }
        else if (@event.IsActionPressed("open_info"))
        {
            OpenInfoPane();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /*	public override void _Process(double delta)
        {
        } */
}
