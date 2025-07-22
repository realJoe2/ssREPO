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
	public async void Spawn()
	{
		float delay = GetIndex() * .067F;
		if(delay > 0)
			await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		enemyInstance = (Node3D) enemyScene.Instantiate();
		AddChild(enemyInstance);
	}
	public void Defeated()
	{
		if(GetChildCount() < 1)
			return;
		GetParent().Call("Decrement");
		enemyInstance.QueueFree();
	}
}
