using Godot;
using System;

public partial class Trigger : Area3D
{
	//Triggers detect when the player enters or exits them.
	TriggerHandler th = new TriggerHandler();
	[ExportGroup("Triggers")]
	[Export] String[] entered = new String[0];
	[Export] String[] exited = new String[0];
	
	[ExportGroup("Properties")]
	[Export] bool oneShot = false;
	
	public override void _Ready()
	{
		AddChild(th);
	}
	void OnBodyEntered(Node3D n)
	{
		th.Run(entered);
	}
	void OnBodyExited(Node3D n)
	{
		th.Run(exited);
		if(oneShot)
			SetDeferred("monitoring", false); //SetDeferred needs "snake_case".
	}
	
}
