using Godot;
using System;

public partial class CharacterPhysics : CharacterBody3D
{
	[Export] float gravityScale = 1.0F;
	[Export] byte dragDivisor = 2;
	const float GRAVITY = .8F;
	
	Vector3 momentum = Vector3.Zero;
	Vector3 addedForce = Vector3.Zero;
	float airMultiplier = .5F;
	public override void _PhysicsProcess(double delta)
	{
		momentum = Velocity;

		if(Math.Abs(momentum.X) > 1F)
			momentum.X -= momentum.X * 1/dragDivisor * airMultiplier;
		else
			momentum.X = 0.0F;
		if(Math.Abs(momentum.Z) > 1F)
			momentum.Z -= momentum.Z * 1/dragDivisor * airMultiplier;
		else
			momentum.Z = 0.0F;
		
		if(IsOnFloor())
		{
			momentum.Y = 0;
			airMultiplier = 1F;
		}
		else
		{
			momentum.Y -= GRAVITY * gravityScale;
			airMultiplier = .25F;
		}

		if(IsOnCeiling() && momentum.Y > 0)
			momentum.Y = 0.0F;
		
		momentum += addedForce;
		momentum.X = Math.Clamp(momentum.X, -200.0F, 200.0F);
		momentum.Z = Math.Clamp(momentum.Z, -200.0F, 200.0F);
		momentum.Y = Math.Clamp(momentum.Y, -400.0F, 400.0F);

		Velocity = momentum;
		MoveAndSlide();
	}
	
	public void AddForce(Vector3 n)
	{
		addedForce = n;
	}
	public Vector3 GetMomentum()
	{
		return momentum;
	}
	public void ResetY()
	{
		momentum.Y = 0;
		Velocity = momentum;
	}
	
}
