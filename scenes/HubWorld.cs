using Godot;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using FileAccess = Godot.FileAccess;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

enum GameState
{
    Hub, Battle, Info, Exit
}
public partial class HubWorld : Node2D, ISaveable
{
    [Export] public PackedScene EnemyScene;
    [Export] public Node2D EnemyContainer;
    [Export] public Vector2 SpawnMin = new(0, 0);
    [Export] public Vector2 SpawnMax = new(1152, 656);
    [Export] private Timer enemyActionTimer;

    private Player player;
    private BattleUI battleUI;
    private InfoPane infoPane;
    private ExitPane exitPanel;
    private Camera2D camera;

    private GameState State;

    private const int ENEMY_COUNT = 5;
    private RandomNumberGenerator rng = new();

    private Character opponent = null;

    private const int NUMBER_OF_MOVES = 5;

    public override void _Ready()
    {
        rng.Randomize();

        player = GetNode<Player>("Player");
        battleUI = GetNode<BattleUI>("CanvasLayer/BattleUI");
        infoPane = GetNode<InfoPane>("CanvasLayer/InfoPane");
        exitPanel = GetNode<ExitPane>("CanvasLayer/ExitPane");

        camera = GetNode<Camera2D>("Player/MainCamera");
        camera.LimitLeft = (int)SpawnMin.X;
        camera.LimitTop = (int)SpawnMin.Y;
        camera.LimitRight = (int)SpawnMax.X;
        camera.LimitBottom = (int)SpawnMax.Y;
        camera.LimitEnabled = true;

        //camera.Position = player.Position;

        player.EnterBattle += StartBattle;
        infoPane.UpdateWords += RefreshInfoPane;
        infoPane.Hidden += () => { ChangeState(GameState.Hub); };
        battleUI.Hidden += () => { ChangeState(GameState.Hub); };
        exitPanel.QuitGame += SaveAndQuit;
        exitPanel.SaveGame += () => { Save(Globals.SAVE_FILE_PATH); };

        player.Position = player.Position.Snapped(Globals.TILE_SIZE) + (Vector2.One * (Globals.TILE_SIZE / 2));

        SpawnEnemies();

        battleUI.PlayerAttack += PlayerAttack;
        battleUI.PlayerDefend += PlayerDefend;

        battleUI.PlayerWin += PlayerWin;
        battleUI.EnemyWin += PlayerLose;

        Ready += () => Load(Globals.SAVE_FILE_PATH);


        enemyActionTimer.Timeout += TriggerEnemyAction;
    }

    public void PlayerWin()
    {
        player.BattlesWon++;
        RespawnEnemies();
        ExitBattle();
    }

    public void PlayerLose()
    {
        ExitBattle();
    }

    public void PlayerAttack(string word)
    {
        if (OS.IsDebugBuild())
        {
            GD.Print($"Player attacking with {word}");
        }
        battleUI.LogAttack(word, Target.Enemy);
        battleUI.LogAttackStatus(player.Attack(new Move(word), opponent), Target.Enemy);
        battleUI.UpdateEnemyHealth(opponent.PercentHealth);
        player.SelectMoves(NUMBER_OF_MOVES, target: opponent);
        battleUI.UpdateMoves(player.CurrentMoves);
        //GetTree().CreateTimer(2);
        //TriggerEnemyAction();
        battleUI.WaitForEnemyMove();
        enemyActionTimer.Start();
    }

    public void PlayerDefend(string word)
    {
        if (OS.IsDebugBuild())
        {
            GD.Print($"Player defend {word}");
        }
        player.Defend(word);
        battleUI.LogDefend(word, Target.Player);
        battleUI.UpdatePlayerLog(player);
        player.SelectMoves(NUMBER_OF_MOVES, target: opponent);
        battleUI.UpdateMoves(player.CurrentMoves);

        battleUI.WaitForEnemyMove();
        enemyActionTimer.Start();
    }

    private void TriggerEnemyAction()
    {

        var moveType = opponent.SelectMoveType();
        Move word;
        switch (moveType)
        {
            case MoveType.Attack:
                word = opponent.SelectRandomMove(player, type: MoveType.Attack);
                var status = opponent.Attack(word, player);
                battleUI.UpdatePlayerHealth(player.PercentHealth);
                battleUI.LogAttack(word, Target.Player);
                battleUI.LogAttackStatus(status, Target.Player);
                break;
            case MoveType.Defend:
                word = opponent.SelectRandomMove(player, type: MoveType.Defend);
                opponent.Defend(word);
                battleUI.LogDefend(word, Target.Enemy);
                battleUI.UpdateEnemyLog(opponent);
                break;
        }
        battleUI.PlayerTurn();
    }

    public override void _Notification(int what)
    {
        switch ((long)what)
        {
            case NotificationWMCloseRequest:
                //SaveAndQuit();
                Quit();
                break;
            default:
                break;

        }
    }
    [DoesNotReturn]
    private void SaveAndQuit()
    {
        Save(Globals.SAVE_FILE_PATH);
        Quit();
    }

    [DoesNotReturn]
    private void Quit() => GetTree().Quit();

    private void SpawnEnemies()
    {
        if (EnemyContainer != null)
            foreach (Node child in EnemyContainer.GetChildren())
                child.Free();

        if (EnemyContainer == null)
        {
            EnemyContainer = new Node2D { Name = "EnemyContainer" };
            AddChild(EnemyContainer);
            EnemyContainer.Owner = this;
        }

        for (int i = 0; i < ENEMY_COUNT; i++)
        {
            var enemy = EnemyScene.Instantiate<Enemy>();
            float x = rng.RandfRange(SpawnMin.X, SpawnMax.X);
            float y = rng.RandfRange(SpawnMin.Y, SpawnMax.Y);
            enemy.Name = $"Enemy_{i}";
            enemy.Position = new Vector2(x, y);
            EnemyContainer.AddChild(enemy);
        }

        if (OS.IsDebugBuild())
        {
            GD.Print($"Spawned {ENEMY_COUNT} enemies at random positions.");
        }
    }

    private void StartBattle(Character enemy)
    {
        //player.CanMove = false;
        //player.SelectMoves(5);
        //battleUI.UpdateMoves(player.CurrentMoves);
        //battleUI.UpdateEnemyHealth(100);
        //battleUI.UpdatePlayerHealth(player.PercentHealth);
        //battleUI.Show();
        if (State == GameState.Hub)
        {
            opponent = enemy;
            player.SelectMoves(NUMBER_OF_MOVES, target: opponent);

            battleUI.UpdateMoves(player.CurrentMoves);
            battleUI.UpdateEnemyHealth(enemy.PercentHealth);
            battleUI.UpdatePlayerHealth(player.PercentHealth);

            battleUI.UpdatePlayerLog(player);
            battleUI.UpdateEnemyLog(opponent);

            battleUI.Show();
            ChangeState(GameState.Battle);
        }
    }

    private void RefreshInfoPane()
    {
        infoPane.ClearWords();
        foreach (var move in player.KnownMoves)
            infoPane.AddWord(move.Word);
    }
    private void OpenInfoPane()
    {
        if (State == GameState.Hub)
        {
            GD.Print(string.Join(", ", player.KnownMoves));
            RefreshInfoPane();
            infoPane.Show();
            ChangeState(GameState.Info);
        }
    }

    private void ExitBattle()
    {
        // TODO: reset health

        player.FullHeal();
        player.ResetArmor();
        battleUI.Close();

        Save(Globals.SAVE_FILE_PATH);
    }
    public override void _Input(InputEvent e)
    {
        if (e.IsActionPressed("ui_cancel"))
        {
            switch (State)
            {
                case GameState.Hub:
                    ChangeState(GameState.Exit);
                    exitPanel.Show();
                    break;
                case GameState.Exit:
                    ChangeState(GameState.Hub);
                    exitPanel.Hide();
                    break;
                case GameState.Battle:
                    //ExitBattle();

                    opponent.FullHeal();
                    opponent.ResetArmor();
                    player.FullHeal();
                    player.ResetArmor();
                    battleUI.Close();
                    break;
                case GameState.Info:
                    infoPane.Hide();
                    break;
            }
        }
        else if (e.IsActionPressed("open_info"))
        {
            OpenInfoPane();
            //infoPane.Show();
            //RefreshInfoPane();
            //player.CanMove = false;
        }
        else if ( State== GameState.Battle&& OS.IsDebugBuild() && e.IsAction("debug_damage"))
        {
            player.DebugSetHealth(1);
            opponent.DebugSetHealth(1);

            battleUI.UpdatePlayerHealth(player.PercentHealth);
            battleUI.UpdateEnemyHealth(opponent.PercentHealth);
        }
    }
    private void ChangeState(GameState state)
    {
        this.State = state;
        switch (state)
        {
            case GameState.Hub:
                player.CanMove = true;
                break;
            case GameState.Battle:
            case GameState.Info:
            case GameState.Exit:
                player.CanMove = false;
                break;
        }
    }


    private void Save(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        using var saveFile = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        //var json_data = Json.Stringify(this.Save());

        var json_data = JsonSerializer.Serialize(Save());
        saveFile.StoreLine(json_data);

    }
    private void Load(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        using var saveFile = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (saveFile is null) return;
        var json_data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(saveFile.GetAsText());
        Load(json_data);
    }

    public void RespawnEnemies() => SpawnEnemies();

    public Dictionary<string, object> Save()
    {

        var data = new Dictionary<string, object>
        {
            {player.Name, player.Save() }
        };
        foreach (var enemy in EnemyContainer.GetChildren().OfType<Enemy>())
        {
            data[enemy.Name] = enemy.Save();
        }
        return data;
    }

    public void Load(Dictionary<string, JsonElement> dict)
    {
        if (OS.IsDebugBuild())
        {
            GD.Print([.. dict]);
            GD.Print(player is null);

        }
        player.Load(dict[player.Name].Deserialize<Dictionary<string, JsonElement>>());
        player.FullHeal();
        foreach (var enemy in EnemyContainer.GetChildren().OfType<Enemy>())
        {
            enemy.Load(dict[enemy.Name].Deserialize<Dictionary<string, JsonElement>>());
            enemy.FullHeal();
        }

        //camera.Position = player.Position; // This could be fixed by loading during node ready, but this works for now
    }
}