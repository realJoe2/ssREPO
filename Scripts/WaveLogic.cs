using Godot;
using System;

public partial class WaveLogic : Node
{	
	byte state = 0;
	const byte INACTIVE = 0;
	const byte ACTIVE = 1;
	const byte COMPLETE = 2;
	
	TriggerHandler th = new TriggerHandler();
	[ExportGroup("Triggers")]
	[Export] String[] onStarted = new String[0];
	[Export] String[] onCompleted = new String[0];
	
	Node3D[] children;
	public override void _Ready()
	{
		state = INACTIVE;
		children = new Node3D[GetChildCount()];
		var b = GetChildren();
		for(int i = 0; i < children.Length; i++)
			children[i] = (Node3D)b[i];
		
	}
	public void Start()
	{
		if(state != INACTIVE)
			return;
		ChangeState(ACTIVE);
		for(int i = 0; i < children.Length; i++)
			children[i].Call("Spawn");
	}
	
	public override void _Process(double delta)
	{
		if(state == ACTIVE && GetChildCount() < 1)
			ChangeState(COMPLETE);
		//GD.Print(state);
	}
	
	private void ChangeState(byte b)
	{
		if(b == state)
		{
			return;
		}
		state = b;
		switch(state)
		{
			case ACTIVE:
				th.Run(onStarted);
				GD.Print("Wave started.");
				break;
			case COMPLETE:
				GD.Print("Wave completed.");
				th.Run(onCompleted);
				QueueFree();
				break;
			default:
				break;
		}
	}
}
