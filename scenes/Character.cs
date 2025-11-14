using Godot;
using System;

public partial class Character : Area2D
{

    private Vector2 position;
    private int currentHealth;

    public float attack_multiplier;
    public float defense_multiplier;

    public void attack()
    {
        throw new NotImplementedException();
    }

    public void defend()
    {
        throw new NotImplementedException();
    }

    public float evaluate_effectiveness()
    {
        throw new NotImplementedException();
    }

    public bool is_alive()
    {
        return (currentHealth > 0) ? true : false;
    }

    public void take_damage(float damage)
    {
        currentHealth -= (int)damage;
    }
}
