using Godot;
using System;

public partial class GameManager : Node
{
	byte gameState = 0;
	const byte START_MENU = 0;
	const byte PAUSE_MENU = 1;
	const byte IN_GAME = 2;
	public override void _Ready()
	{
		//start by putting up a black loading screen
		GD.Randomize();
		Engine.MaxFps = 180;
		//precompile shaders, load the start menu
		//remove loading screen after finished
		
	}
	
	void ChangeState(byte s)
	{
		if(s == gameState)
			return;
		gameState = s;
		
		switch(gameState)
		{
			case START_MENU:
				break;
			case PAUSE_MENU:
				break;
			case IN_GAME:
				break;
		}
	}
}
