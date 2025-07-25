using Godot;
using System;

public partial class OptionsMenu : Control
{
    public override void _Process(double delta)
    {
        if(IsVisible() && Input.IsActionJustPressed("Interact"))
        {
            Hide();
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
        }
    }
}
