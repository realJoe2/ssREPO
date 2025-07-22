using Godot;
using System;

public partial class GameManager : Node
{
	[Export] Resource defaultLevel;

	static GameManager instance = null; //implement GameManager as Singleton
	private GameManager()
	{
	}
	public static GameManager Get()
	{
		if(instance == null)
		{
			instance = new GameManager();
			GD.Print("instanced");
		}
		return instance;
	}
	
	public override void _Ready()
	{
		GD.Randomize();
		Engine.MaxFps = 180;
		ChangeLevel(defaultLevel);
		
	}
	public void SetMaxFPS(int fps)
	{
		Engine.MaxFps = fps;
	}

	Node currentLevel;
	PackedScene next;
	public void ResetLevel()
	{
		GetTree().CallGroup("Resets", "Reset");
	}
	public void ResetLevelFull()
	{
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
			return;
		
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
	
}
