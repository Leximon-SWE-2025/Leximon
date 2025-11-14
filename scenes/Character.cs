using Godot;
using System;

public partial class Character : Area2D
{
	private:
		Vector2 position;
		int hp;
	public:
		float attack_multiplier;
		float defense_multiplier;
	
	public void attack();
	
	public void defend();
	
	public float evaluate_effectiveness();
	
	public bool is_alive() {
		return (hp > 0)? true: false;
	}
	
	public void take_damage(float damage) {
		hp -= damage;
	}
}
