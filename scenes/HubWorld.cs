using Godot;
using System;
using System.Linq;
enum GameState
{
    Hub, Battle, Info, Exit
}
public partial class HubWorld : Node2D
{
    [Export] public PackedScene EnemyScene;  
    [Export] public Node2D EnemyContainer; 
    [Export] public Vector2 SpawnMin = new(0, 0);
    [Export] public Vector2 SpawnMax = new(1152, 656);

    private Player player;
    private BattleUI battleUI;
    private InfoPane infoPane;
    private ExitPane exitPanel;

    private GameState State;

    [Export]
    private const int ENEMY_COUNT = 5;
    private RandomNumberGenerator rng = new();

    public override void _Ready()
    {
        rng.Randomize();

        player = GetNode<Player>("Player");
        battleUI = GetNode<BattleUI>("CanvasLayer/BattleUI");
        infoPane = GetNode<InfoPane>("CanvasLayer/InfoPane");
        exitPanel = GetNode<ExitPane>("CanvasLayer/ExitPane");

        var camera = GetNode<Camera2D>("Player/MainCamera");
        camera.LimitLeft = (int)SpawnMin.X;
        camera.LimitTop = (int)SpawnMin.Y;
        camera.LimitRight = (int)SpawnMax.X;
        camera.LimitBottom = (int)SpawnMax.Y;
        camera.LimitEnabled = true;

        player.EnterBattle += StartBattle;
        infoPane.UpdateWords += RefreshInfoPane;
        infoPane.Hidden += () => { ChangeState(GameState.Hub); };
        battleUI.Hidden += () => { ChangeState(GameState.Hub); };
        exitPanel.QuitGame += () => GetTree().Quit();

        player.Position = player.Position.Snapped(Globals.TILE_SIZE) + (Vector2.One * (Globals.TILE_SIZE / 2));

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (EnemyContainer != null)
            foreach (Node child in EnemyContainer.GetChildren())
                child.QueueFree();

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
            enemy.Position = new Vector2(x, y);
            EnemyContainer.AddChild(enemy);
        }

        GD.Print($"Spawned {ENEMY_COUNT} enemies at random positions.");
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
            player.SelectMoves(5);

            battleUI.UpdateMoves(player.CurrentMoves);
            battleUI.UpdateEnemyHealth(100f);
            battleUI.UpdatePlayerHealth(player.PercentHealth);

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
        infoPane.Show();
        ChangeState(GameState.Info);
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
    public void RespawnEnemies() => SpawnEnemies();
}