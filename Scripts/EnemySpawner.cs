using Godot;
using System;

public partial class EnemySpawner : Node3D
{
	[Export] Resource enemyResource;
	PackedScene enemyScene;
	
	public override void _Ready()
	{
		enemyScene = QuickFetch.Fetch(enemyResource);
	}

	Node3D enemyInstance;
	float delay;
	public async void Spawn()
	{
		delay = (float) (GetIndex() * 1/Engine.GetFramesPerSecond()); //doing this makes it so each enemy spawns with a frame of delay between the last
		if(delay > 0)
			await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		enemyInstance = (Node3D) enemyScene.Instantiate();
		AddChild(enemyInstance);
	}
	Node e;
	public void Defeated()
	{
		if(GetChildCount() < 1)
			return;
		GetParent().Call("Decrement");
		if(GetChildCount() <= 0)
			return;
		
		enemyInstance.QueueFree();
	}
}
