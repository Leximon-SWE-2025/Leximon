using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;


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
    private MovePopUp MovePopup;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        enemyHealthBar = GetNode<HealthBar>("EnemyHealth");
        playerHealthBar = GetNode<HealthBar>("PlayerHealth");

        enemyLogPane = GetNode<LogPane>("EnemyLog");
        playerLogPane = GetNode<LogPane>("PlayerLog");

        movePane = GetNode<MovePane>("MovePane");
        DefinitionPopup = GetNode<DefinitionPopUp>("DefinitionPopUp");

        MovePopup = GetNode<MovePopUp>("MovePopUp");

        DefinitionPopup.CloseDefinitionPopUp += () =>
        {
            if (DefinitionPopup.Visible)
            {
                state = BattleUIState.SelectWord;
            }
        };

        MovePopup.CloseMovePopUp += () =>
        {
            if (MovePopup.Visible)
            {
                state = BattleUIState.SelectWord;
            }
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
                //GD.Print(state);
                if (state == BattleUIState.SelectWord)
                {
                    DefinitionPopup.Text = string.Join("\n", WordManager.GetDefinitions(word));
                    DefinitionPopup.Word = WordManager.TitleCaseWord(word);
                    state = BattleUIState.ViewDefinition;
                    DefinitionPopup.Show();
                }
            };

            card.SelectMove += (word) =>
            {
                //GD.Print(state);
                if (state == BattleUIState.SelectWord)
                {
                    MovePopup.Text = WordManager.TitleCaseWord(word);
                    state = BattleUIState.SelectMove;
                    MovePopup.Show();
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

    internal void Close()
    {
        DefinitionPopup.Close();
        MovePopup.Close();

        state = BattleUIState.SelectWord;
        Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(double delta)
    //{
    //}


    [Signal]
    public delegate void SelectPlayerMovesEventHandler(int count);
}
