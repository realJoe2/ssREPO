using Godot;
using System;

public partial class footninjaLogic : Node
{
	[Export] byte moveSpeed = 0;
	[Export] NavigationAgent3D navAgent;
	[Export] Node3D model;
	CharacterBody3D  player;
	CharacterBody3D parent;
	
	byte state = 255;
	const byte IDLE = 0;
	const byte PURSUE_PLAYER = 1;
	const byte ATTACK = 2;
	const byte DEAD = 3;
	//const byte GOOMBA = 4;
	
	byte randomTickOffset;
	public override void _Ready()
	{
		parent = (CharacterBody3D) GetParent();
		var pl = GetTree().GetNodesInGroup("Player");
		player = (CharacterBody3D) pl[0];
		if(navAgent == null)
		{
			GD.PushWarning("NavigationAgent field is empty, adding from path '../NavigationAgent3D'.");
			navAgent = (NavigationAgent3D)GetNode("../NavigationAgent3D");
		}
		
		randomTickOffset = (byte)(GD.Randi() % 6);
		randomTickOffset += 10;
		ChangeState(PURSUE_PLAYER);
	}

	Vector3 nextTarget = Vector3.Zero;
	public override void _PhysicsProcess(double delta)
	{
		float distanceToPlayer = parent.GlobalPosition.DistanceTo(player.GlobalPosition);
		float verticalDistance =  Mathf.Abs(player.GlobalPosition.Y - parent.GlobalPosition.Y);
		switch(state)
		{
			case IDLE:
				LookAtSmooth(player.GlobalPosition, model, .1F);
				if(distanceToPlayer >= 2.5F || verticalDistance >= 2.5F)
					ChangeState(PURSUE_PLAYER);
				//check for conditions that could change the state
				break;

			case PURSUE_PLAYER:
				navAgent.TargetPosition = player.GlobalPosition;
				if(Engine.GetPhysicsFrames() % randomTickOffset == 0)
					nextTarget = navAgent.GetNextPathPosition();
				
				if(distanceToPlayer > 5F)
					LookAtSmooth(nextTarget, model, .1F);
				else
					LookAtSmooth(player.GlobalPosition, model, .1F);
				
				Vector3 movementVector = (nextTarget - parent.GlobalPosition).Normalized();
				movementVector.Y = 0;
				if(parent.IsOnFloor())
					parent.Call("AddForce", movementVector * moveSpeed * .4F);
				else
					parent.Call("AddForce", movementVector * moveSpeed * .1F);
				
				if(distanceToPlayer < 2.5F && verticalDistance < 2.5F)
					ChangeState(IDLE);
				break;

			default:
				break;
		}
	}
	
	public void LookAtSmooth(Vector3 n, Node3D obj, float easing)
	{
		n.Y = obj.GlobalPosition.Y;
		Vector2 xyPosition = new Vector2(obj.GlobalPosition.X, obj.GlobalPosition.Z);
		Vector2 playerXY = new Vector2(n.X, n.Z);
		Vector2 direction = -(xyPosition - playerXY);
		Vector3 newRotation = obj.Rotation;
		newRotation.Y = Mathf.LerpAngle(obj.Rotation.Y, Mathf.Atan2(direction.X, direction.Y), easing);
		obj.Rotation = newRotation;
	}
	private void ChangeState(byte s)
	{
		if(s == state)
			return;
		
		state = s;
		switch(state)
			{
				case IDLE:
					//set animation to idle
					parent.Call("AddForce", Vector3.Zero);
					break;
					
				case ATTACK:
					//begin attack sequence with animation player

					break;
				
				case PURSUE_PLAYER:
					//switch animation to run and do other visual stuff
					break;
				
				case DEAD:
					GetParent().GetParent().Call("Defeated");
					//add death effects as a child of root(?)
					break;
				
				/*case GOOMBA:
					Vector3 hop = Vector3.Zero;
					hop.X = (float)GD.RandRange(-40F, 40F);
					hop.Z = (float)GD.RandRange(-40F, 40F);
					hop.Y = (float)GD.RandRange(0F, 8F);
					player.Call("AddForce", hop);
					ChangeState(PURSUE_PLAYER);
					break;*/
			}
	}
}
