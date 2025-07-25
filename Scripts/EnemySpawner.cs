using Godot;
using System;

public partial class EnemySpawner : Node3D
{
	[Export] string enemyResourcePath;
	PackedScene enemyScene;
	
	public override void _Ready()
	{
		enemyScene = QuickFetch.Fetch(enemyResourcePath);
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
		GD.Print("Spawned enemy of type " + enemyInstance.Name);
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
	public void Reset()
	{
		if(GetChildCount() < 1)
			return;
		if(GetChildCount() > 1) //exception handling
		{
			GD.PushWarning("Enemy spawner has more than one child. Exception handled..");
			for(int i = 0; i < GetChildCount(); i++)
				GetChild(i).QueueFree();
			return;
		}
		GetChild(0).QueueFree();
	}
}
