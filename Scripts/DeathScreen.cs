using Godot;
using System;

public partial class DeathScreen : Control
{
    AudioStreamPlayer deathMusic;
    AudioStreamPlayer deathSound;
    public override void _Ready()
    {
        deathMusic = (AudioStreamPlayer) GetNode("Death Music");
        deathSound  = (AudioStreamPlayer) GetNode("Death Sound");
    }
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
    void PlayDeathAudio()
    {
        if(IsVisible())
        {
            deathSound.Play();
            deathMusic.Play();
        }
        else
        {
            deathSound.Stop();
            deathMusic.Stop();
        }
    }
}
