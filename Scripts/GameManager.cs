using Godot;
using System;

public partial class GameManager : Node
{
	[Export] string defaultLevel;
	Control deathScreen;
	Control loadingScreen;
	Control optionsMenu;
	[Export(PropertyHint.Range, "30,300,")] int maxFramesPerSecond = 180;
	
	public override void _Ready()
	{
		GD.Randomize();
		Engine.MaxFps = maxFramesPerSecond;
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
		GD.Print("Soft reset level " + currentLevel.Name);
	}
	public void ResetLevelFull()
	{
		if(next == null)
		{
			GD.Print("No level to reset to");
			return;
		}
		currentLevel.Free();
		Node nextLevel = next.Instantiate();
		AddChild(nextLevel);
		currentLevel = nextLevel;
		GD.Print("Hard reset level: " + currentLevel.Name);
	}
	public async void ChangeLevel(string levelPath)
	{
		if(levelPath == null)
		{
			GD.PushError("Level field is null");
			return;
		}

		next = QuickFetch.Fetch(levelPath);
		if(next == null)
		{
			GD.PushWarning("Level " + next + " not found");
			return;
		}
		loadingScreen.Show();
		await ToSignal(GetTree().CreateTimer(1/Engine.GetFramesPerSecond() + .001), SceneTreeTimer.SignalName.Timeout); //wait a frame so that the loading screen renders
		if(currentLevel != null)
			currentLevel.QueueFree();
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		Node nextLevel = next.Instantiate();
		currentLevel = nextLevel;
		AddChild(nextLevel);
		GD.Print("Changed level to '" + nextLevel.Name + "'");
		loadingScreen.Hide();
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel") && currentLevel.Name != "Main Menu")
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
