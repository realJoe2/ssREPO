using Godot;
using System;

public partial class PlayerMovementLogic : Node
{
	[Export] byte moveSpeed = 0;
	[Export] byte jumpHeight = 0;
	
	byte state = 0;
	const byte GROUNDED = 0;
	const byte AIRBORNE = 1;
	const byte DEAD = 3;
	
	CharacterBody3D parent;
	Node3D cameraPivot;
	Camera3D camera3D;
	
	public override void _Ready()
	{
		parent = (CharacterBody3D)GetParent();
		cameraPivot = GetNode<Node3D>("../CameraPivot");
		camera3D = GetNode<Camera3D>("../CameraPivot/FirstPersonCamera");
		state = AIRBORNE;
	}
	
	float sensitivity = 3/1000F;
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
			cameraPivot.RotateY(-mouseMotion.Relative.X * sensitivity);
			camera3D.RotateX(-mouseMotion.Relative.Y * sensitivity);
			Vector3 cameraClamp = camera3D.Rotation;
			cameraClamp.X = Mathf.Clamp(cameraClamp.X, Mathf.DegToRad(-92.0F), Mathf.DegToRad(90.0F));
			camera3D.Rotation = cameraClamp;
		}
	}
	
	Vector3 wishDirection = Vector3.Zero;
	byte bufferFrames, coyoteFrames = 0;
	public override void _PhysicsProcess(double delta)
	{
		if(bufferFrames > 0)
			bufferFrames--;
		if(coyoteFrames > 0)
			coyoteFrames--;
			
		wishDirection.X = Input.GetAxis("MoveLeft", "MoveRight");
		wishDirection.Z = Input.GetAxis("MoveForward", "MoveBack");
		wishDirection = (cameraPivot.Transform.Basis * wishDirection).Normalized();
		switch(state)
		{
			case GROUNDED:
				parent.Call("AddForce", wishDirection * moveSpeed * .4F);
				
				if(Input.IsActionJustPressed("Jump") || bufferFrames > 0)
				{
					parent.Call("AddForce", jumpHeight * Vector3.Up);
					bufferFrames = 0;
				}
				if((bool)parent.IsOnFloor() == false)
					ChangeState(AIRBORNE);
				break;
			case AIRBORNE:
				parent.Call("AddForce", wishDirection * moveSpeed * .1F);
				if(Input.IsActionJustPressed("Jump"))
				{
					bufferFrames = 8;
					if(coyoteFrames > 0)
					{
						Vector3 pMomentum = (Vector3)parent.Call("GetMomentum");
						parent.Call("ResetY");
						parent.Call("AddForce", Vector3.Up * jumpHeight);
					}
				}
				if((bool)parent.IsOnFloor() == true)
					ChangeState(GROUNDED);
				break;
		}
	}
	public void ChangeState(byte b)
	{
		if (b == state)
			return;
		state = b;

		switch (state)
		{
			case GROUNDED:
				//play a jump sound maybe?
				break;

			case AIRBORNE:
				Vector3 pMomentum = (Vector3) parent.Call("GetMomentum");
				if (pMomentum.Y < 0)
					coyoteFrames = 8;
				//play a land sound maybe?
				break;

			case DEAD:
				GD.Print("Died!");
				var gameManager = GetTree().GetNodesInGroup("Game Manager")[0];
				gameManager.Call("PlayerDeath");
				break;

			default:
				break;
		}
	}

	public void Reset()
	{
		cameraPivot.Rotation = Vector3.Zero;
		camera3D.GlobalRotation = Vector3.Zero;
		ChangeState(GROUNDED);
	}
}
