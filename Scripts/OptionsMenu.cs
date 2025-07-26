using Godot;
using System;

public partial class OptionsMenu : Control
{
    GameManager gameManager;
    //Button resumeButton;
    public override void _Ready()
    {
        gameManager = (GameManager) GetTree().GetNodesInGroup("Game Manager")[0];
    }
    void OnKillButton()
    {
        Hide();
        gameManager.PlayerDeath();
    }
    void OnResumeButton()
    {
        //await ToSignal(GetTree().CreateTimer(1/Engine.GetFramesPerSecond() + .001F), SceneTreeTimer.SignalName.Timeout); //prevent input
        GetTree().Paused = false;
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        Hide();
    }
}
