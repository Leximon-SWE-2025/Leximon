using Godot;
using System;

public partial class HealthBar : ProgressBar
{
    public void SetHealth(double value)
    {
        Value = value;
    }
}
