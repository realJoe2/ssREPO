using Godot;
using System;

public partial class OptionsMenu : Control
{
    GameManager gameManager;
    public override void _Ready()
    {
        gameManager = (GameManager) GetTree().GetNodesInGroup("Game Manager")[0];
    }
    public override void _Process(double delta)
    {
        if(IsVisible() && Input.IsActionJustPressed("Interact"))
        {
            Hide();
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
        }
    }
    void OnKillButton()
    {
        Hide();
        gameManager.PlayerDeath();
    }
}
