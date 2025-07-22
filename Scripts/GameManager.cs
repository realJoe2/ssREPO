using Godot;
using System;

public partial class GameManager : Node
{
	[Export] Resource defaultLevel;
	
	public override void _Ready()
	{
		GD.Randomize();
		Engine.MaxFps = 180;
		ChangeLevel(defaultLevel);
	}

	Node currentLevel;
	PackedScene next;
	public void ResetLevel()
	{
		GetTree().CallGroup("Resets", "Reset");
	}
	public void ResetLevelFull()
	{
		if(next == null)
		{
			GD.Print("No level to reset to.");
			return;
		}
		currentLevel.QueueFree();
		Node nextLevel = next.Instantiate();
		AddChild(nextLevel);
		currentLevel = nextLevel;
	}
	public void ChangeLevel(Resource level)
	{
		if(level == null)
		{
			GD.PushError("Level field is null.");
			return;
		}

		next = QuickFetch.Fetch(level);
		if(next == null)
		{
			GD.PushWarning("Level " + level + " not found.");
			return;
		}
		
		if(currentLevel != null)
			currentLevel.QueueFree();
		
		Node nextLevel = next.Instantiate();
		currentLevel = nextLevel;
		AddChild(nextLevel);
		GD.Print("Changed level to '" + nextLevel.Name + "'.");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Interact"))
			ResetLevel();
	}

	public void PlayerDeath()
	{
		ResetLevel();
		//pause and put up death screen
		//unpause and remove death screen after player presses "Interact"
	}

	public void SetMaxFPS(int fps)
	{
		Engine.MaxFps = fps;
	}
}
