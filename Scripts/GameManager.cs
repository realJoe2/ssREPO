using Godot;
using System;

public partial class GameManager : Node
{
	[Export] Resource defaultLevel;
	Control deathScreen;
	Control loadingScreen;
	Control optionsMenu;
	
	public override void _Ready()
	{
		GD.Randomize();
		Engine.MaxFps = 180;
		deathScreen = (Control) GetNode("Top Layer/Death Screen");
		loadingScreen = (Control) GetNode("Top Layer/Loading Screen");
		optionsMenu = (Control) GetNode("Top Layer/Options Menu");
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
	public async void ChangeLevel(Resource level)
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
		loadingScreen.Show();
		await ToSignal(GetTree().CreateTimer(1/Engine.GetFramesPerSecond() + .001), SceneTreeTimer.SignalName.Timeout);
		if(currentLevel != null)
			currentLevel.QueueFree();
		
		Node nextLevel = next.Instantiate();
		currentLevel = nextLevel;
		AddChild(nextLevel);
		GD.Print("Changed level to '" + nextLevel.Name + "'.");
		loadingScreen.Hide();
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel"))
		{
			optionsMenu.Show();
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			GetTree().Paused = true;
		}
	}

	public void PlayerDeath()
	{
		deathScreen.Show();
		GetTree().Paused = true;
		Input.SetMouseMode(Input.MouseModeEnum.Visible);
	}

	public void SetMaxFPS(int fps)
	{
		Engine.MaxFps = fps;
	}
}
