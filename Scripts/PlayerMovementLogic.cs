using Godot;
using System;

public partial class PlayerMovementLogic : Node
{
	[Export] byte moveSpeed = 4;
	[Export] byte jumpHeight = 1;
	
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
	
	float sensitivity = 3/1000F; //make this editable in settings
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
	public override void _PhysicsProcess(double delta)
	{
		wishDirection.X = Input.GetAxis("MoveLeft", "MoveRight");
		wishDirection.Z = Input.GetAxis("MoveForward", "MoveBack");
		wishDirection = (cameraPivot.Transform.Basis * wishDirection).Normalized();
		switch(state)
		{
			case GROUNDED:
				if(Input.IsActionPressed("Jump"))
					wishDirection.Y = jumpHeight;
				parent.Call("AddForce", wishDirection * moveSpeed);

				if((bool)parent.IsOnFloor() == false)
					ChangeState(AIRBORNE);
				break;
			case AIRBORNE:
				wishDirection.Y = 0F;
				parent.Call("AddForce", wishDirection * moveSpeed);

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
}
