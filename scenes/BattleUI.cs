using Godot;
using System;
using System.Collections.Generic;


enum BattleUIState
{
    SelectWord, ViewDefinition, SelectMove
}
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

    private BattleUIState state;

    private MovePane movePane; private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    private DefinitionPopUp DefinitionPopup;
    private MovePopUp MovePopUp;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        enemyHealthBar = GetNode<HealthBar>("EnemyHealth");
        playerHealthBar = GetNode<HealthBar>("PlayerHealth");

        enemyLogPane = GetNode<LogPane>("EnemyLog");
        playerLogPane = GetNode<LogPane>("PlayerLog");

        movePane = GetNode<MovePane>("MovePane");
        DefinitionPopup = GetNode<DefinitionPopUp>("DefinitionPopUp");

        MovePopUp = GetNode<MovePopUp>("MovePopUp");

        DefinitionPopup.CloseDefinitionPopUp += () =>
        {
            GD.Print("Close Def");
            state = BattleUIState.SelectWord;
            DefinitionPopup.Hide();
        };

        MovePopUp.CloseMovePopUp += () =>
        {
            GD.Print("Close Move");
            state = BattleUIState.SelectWord;
            MovePopUp.Hide();
        };

        //TODO: grab player refrence

        //player.UpdateHealth += UpdatePlayerHealth;
        //UpdatePlayerHealth();
        this.VisibilityChanged += () => { if (this.Visible) state = BattleUIState.SelectWord; };
    }

    public void UpdateMoves(Move[] moves)
    {
        movePane.ClearMoves();
        foreach (var move in moves)
        {
            var card = movePane.AddMove(move);

            card.ShowDefinition += (word) =>
            {
                GD.Print(state);
                if (state == BattleUIState.SelectWord)
                {
                    DefinitionPopup.Text = $"Definition for {word}";
                    state = BattleUIState.ViewDefinition;
                    DefinitionPopup.Show();
                }
            };

            card.SelectMove += (word) =>
            {
                GD.Print(state);
                if (state == BattleUIState.SelectWord)
                {
                    MovePopUp.Text = $"{word}";
                    state = BattleUIState.SelectMove;
                    MovePopUp.Show();
                }
            };

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
