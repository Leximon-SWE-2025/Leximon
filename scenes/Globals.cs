using System;

internal class Globals
{
    public static readonly int TILE_SIZE = 16;

    public static TimeSpan MovementTime => TimeSpan.FromSeconds(0.15);
    public static double MovementTimeSec => MovementTime.TotalSeconds;
}

