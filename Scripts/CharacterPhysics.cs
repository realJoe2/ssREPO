using Godot;
using System;

public partial class CharacterPhysics : CharacterBody3D
{
	[Export] float gravityScale = 1.0F;
	[Export] public float dragDivisor = 2F;
	[Export] float maxSpeed = 200F;

	const float GRAVITY = .8F;
	const int deltaMultiplier = 120; //using delta time slows everything down, so this constant follows it.

	Vector3 momentum = Vector3.Zero;
	Vector3 addedForce = Vector3.Zero;

	public override void _Process(double delta)
	{
		momentum = Velocity;

		if(Math.Abs(momentum.X) > 1F)
			momentum.X -= (momentum.X * 1/dragDivisor * deltaMultiplier) * (float) delta;
		else
			momentum.X = 0.0F;
		if(Math.Abs(momentum.Z) > 1F)
			momentum.Z -= (momentum.Z * 1/dragDivisor * deltaMultiplier) * (float) delta;
		else
			momentum.Z = 0.0F;
		
		if(IsOnFloor())
			momentum.Y = 0;
		else
			momentum.Y -= (GRAVITY * gravityScale * deltaMultiplier) * (float) delta;

		if(IsOnCeiling() && momentum.Y > 0)
			momentum.Y = 0.0F;
		
		momentum += addedForce * (float) delta * deltaMultiplier;
		momentum = momentum.LimitLength(maxSpeed); //this stops any crazy tech from letting the player go TOO fast

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
	public void SetDrag(float n)
	{
		dragDivisor = n;
	}
	public float GetDrag()
	{
		return dragDivisor;
	}
	
}
