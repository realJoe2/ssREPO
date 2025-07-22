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
	public async void Spawn()
	{
		float delay = GetIndex() * .1F;
		if(delay > 0)
			await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		Node3D enemyInstance = (Node3D) enemyScene.Instantiate();
		AddChild(enemyInstance);
	}
	public void Defeated()
	{
		//free the spawner and do other stuff idk
		QueueFree();
	}
}
