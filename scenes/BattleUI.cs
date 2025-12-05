using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

public enum Target
{
    Player, Enemy
}
enum BattleUIState
{
    SelectWord, ViewDefinition, SelectMove
}
public partial class BattleUI : Control
{
    private HealthBar enemyHealthBar;
    private HealthBar playerHealthBar;

    private LogPane enemyLogPane;
    private LogPane playerLogPane;

    private BattleUIState state;

    private MovePane movePane; private int currentHealth;
    public int CurrentHealth => currentHealth;

    private int maxHealth;
    public int MaxHealth => maxHealth;

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

        MovePopup.PlayerAttack += (word) => { EmitSignal(SignalName.PlayerAttack, word); };
        MovePopup.PlayerDefend += (word) => { EmitSignal(SignalName.PlayerDefend, word); };



        //TODO: grab player refrence

        //player.UpdateHealth += UpdatePlayerHealth;
        //UpdatePlayerHealth();
        this.VisibilityChanged += () => { if (this.Visible) state = BattleUIState.SelectWord; };
    }



    public void UpdatePlayerLog(Character player)
    {
        GD.Print(player.Type);
        playerLogPane.Label = $"Player ({player.Type}: {player.Armor:0.00})";
    }
    public void UpdateEnemyLog(Character enemy)
    {
        GD.Print(enemy.Type);
        enemyLogPane.Label = $"Enemy ({enemy.Type}: {enemy.Armor:0.00})";
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

    public void UpdatePlayerHealth(double healthPercent)
    {
        playerHealthBar.SetHealth(healthPercent);
        if (healthPercent <= 0)
        {
            EmitSignal(SignalName.EnemyWin);
        }
    }
    public void UpdateEnemyHealth(double healthPercent)
    {
        enemyHealthBar.SetHealth(healthPercent);
        if (healthPercent <= 0) {
            EmitSignal(SignalName.PlayerWin);
        }
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

    LogPane GetLogPane(Target target) => target switch
    {
        Target.Player => playerLogPane,
        Target.Enemy => enemyLogPane,
        _ => throw new NotImplementedException(),
    };


    public void LogAttack(string word, Target target) => GetLogPane(target).LogMove(word, MoveType.Attack);

    public void LogDefend(string word, Target target) => GetLogPane(target).LogMove(word, MoveType.Defend);


    public void LogAttackStatus(AttackStatus status, Target target)
    {
        GetLogPane(target).Log($"Attack was {status}");
    }

    [Signal]
    public delegate void PlayerWinEventHandler();
    [Signal]
    public delegate void EnemyWinEventHandler();

    [Signal]
    public delegate void SelectPlayerMovesEventHandler(int count);
    [Signal]
    public delegate void PlayerAttackEventHandler(string word);
    [Signal]
    public delegate void PlayerDefendEventHandler(string word);

    [Signal]
    public delegate void EnemyAttackEventHandler(string word);
    [Signal]
    public delegate void EnemyDefendEventHandler(string word);
}
