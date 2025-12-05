using Godot;
using System;
using System.IO;

internal static class Globals
{
    public static readonly int TILE_SIZE = 16;

    public static string SAVE_FILE_PATH => "user://savegame.save";

    public static TimeSpan MovementTime => TimeSpan.FromSeconds(0.15);
    public static double MovementTimeSec => MovementTime.TotalSeconds;

    public static readonly int BasePlayerDamage = 10;
    public static readonly int BaseEnemyDamage = 7;
    public static readonly float BaseDefence = 1;
    public static readonly float BaseDefenceIncrease = 0.07f;

    public static readonly int WinsToSwitchType = 3;

}

public enum MoveType
{
    Attack, Defend
}