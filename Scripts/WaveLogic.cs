using Godot;
using System;

public partial class WaveLogic : Node
{	
	byte state = 0;

	byte enemiesLeft;
	const byte INACTIVE = 0;
	const byte ACTIVE = 1;
	const byte COMPLETE = 2;
	[Signal] public delegate void WaveStartedEventHandler();
	[Signal] public delegate void WaveCompletedEventHandler();
	
	Node3D[] children;
	public override void _Ready()
	{
		state = INACTIVE;
		children = new Node3D[GetChildCount()];
		var b = GetChildren();
		for(int i = 0; i < children.Length; i++)
			children[i] = (Node3D)b[i];
			
		enemiesLeft = (byte) children.Length;
	}
	public void Decrement()
	{
		if(enemiesLeft > 0)
			enemiesLeft--;
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
		if(state == ACTIVE && enemiesLeft < 1)
			ChangeState(COMPLETE);
		//GD.Print(state);
	}
	public void Reset()
	{
		state = INACTIVE;
		for(int i = 0; i < children.Length; i++)
			children[i].Call("Defeated");
		enemiesLeft = (byte) children.Length;
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
				//th.Run(onStarted);
				EmitSignal(SignalName.WaveStarted);
				GD.Print("Wave started.");
				break;
			case COMPLETE:
				GD.Print("Wave completed.");
				EmitSignal(SignalName.WaveCompleted);
				break;
			default:
				break;
		}
	}
}
