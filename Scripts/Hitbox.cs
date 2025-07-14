using Godot;
using System;

public partial class Hitbox : Area3D
{
	[Export] Node healthComponent;
	public override void _Ready()
	{
		if(healthComponent == null)
			GD.PushError("Null health component.");
	}
	public void Hit(byte damage)
	{
		healthComponent.Call("TakeDamage", damage);
		//render damage effects, such as blood
	}
}
