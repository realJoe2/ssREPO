using Godot;
using System;

public partial class WaveLogic : Node
{	
	byte enemiesLeft;

	byte state = 0;
	const byte INACTIVE = 0;
	const byte ACTIVE = 1;
	const byte COMPLETE = 2;
	[Signal] public delegate void WaveStartedEventHandler();
	[Signal] public delegate void WaveCompletedEventHandler();
	
	Godot.Collections.Array<Godot.Node> children;
	public override void _Ready()
	{
		state = INACTIVE;
		children = GetChildren();
		enemiesLeft = (byte) children.Count;
	}

	bool threeMode = false;
	public void Decrement()
	{
		if(enemiesLeft > 0)
			enemiesLeft--;

		if(enemiesLeft == 3 && threeMode != true) //threeMode only activates if a wave started with more than three members
		{
			threeMode = true;
			GetTree().CallGroup("Enemy Logic Node", "ActivateThreeMode");
			GD.Print("Activated three mode");
		}
	}
	public void Start()
	{
		if(state != INACTIVE)
			return;
		ChangeState(ACTIVE);
		for(int i = 0; i < children.Count; i++)
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
		
		enemiesLeft = (byte) children.Count;
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
				GD.Print("Wave started");
				break;
			case COMPLETE:
				GD.Print("Wave completed");
				EmitSignal(SignalName.WaveCompleted);
				break;
			default:
				break;
		}
	}
}
