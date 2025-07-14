using Godot;
using System;

public partial class AnimationLogic : AnimationPlayer
{
	public override void _Ready()
	{
		Play("Idle");
	}
}
