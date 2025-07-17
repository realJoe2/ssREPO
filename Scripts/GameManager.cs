using Godot;
using System;

public partial class GameManager : Node
{
	public enum GameState
	{
		StartMenu,
		InGame
	}
	public GameState state;
	public override void _Ready()
	{
		//start by putting up a black loading screen
		GD.Randomize();
		Engine.MaxFps = 180;
		//precompile shaders, load the start menu
		//remove loading screen after finished
		
	}
	public void SetMaxFPS(int fps)
	{
		Engine.MaxFps = fps;
	}
	public void ChangeState(GameState s)
	{
		if(s == state)
			return;
		state = s;
		
		switch(state)
		{
			case GameState.StartMenu:
				break;
			case GameState.InGame:
				break;
		}
	}
}
