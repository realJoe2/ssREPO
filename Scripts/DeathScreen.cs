using Godot;
using System;

public partial class DeathScreen : Control
{
    void OnRetryPressed()
    {
        Shortcut();
    }
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Interact") && IsVisible())
        {
            Shortcut();
        }
    }
    void Shortcut()
    {
        GetParent().GetParent().Call("ResetLevel");
        Hide();
        GetTree().Paused = false;
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
}
