using Godot;
using System;

public partial class OptionsMenu : Control
{
    void OnResumeButtonDown()
    {
        Hide();
        GetTree().Paused = false;
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
}
