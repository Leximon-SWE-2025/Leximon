using Godot;
using System;
using System.IO;

internal class Globals
{
    public static readonly int TILE_SIZE = 16;

    public static string SAVE_FILE_PATH => "user://savegame.save";

    public static TimeSpan MovementTime => TimeSpan.FromSeconds(0.15);
    public static double MovementTimeSec => MovementTime.TotalSeconds;
}

