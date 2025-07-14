using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export] int maxHealth = 1;
	int currentHealth;
	
	[Export] Node logicScript;
	public override void _Ready()
	{
		currentHealth = maxHealth;
	}
	public void TakeDamage(byte damage)
	{
		currentHealth -= damage;
		if(currentHealth <= 0)
			logicScript.Call("ChangeState", 3);
	}
}
