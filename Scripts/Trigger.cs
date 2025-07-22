using Godot;
using System;

public partial class Trigger : Area3D
{
	//Triggers detect when the player enters or exits them.
	//TriggerHandler th = new TriggerHandler();
	[Export] bool oneShot = false;

	void OnBodyExited(Node3D n)
	{
		//th.Run(exited);
		if(oneShot)
			SetDeferred("monitoring", false); //SetDeferred needs "snake_case".
	}

	public void Reset()
	{
		SetDeferred("monitoring", true);
	}
	
}
