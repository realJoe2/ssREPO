using Godot;
using System;

public partial class DeathScreen : Control
{
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Interact") && IsVisible())
        {
            GetParent().GetParent().Call("ResetLevel");
            Hide();
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
        }
    }
}
