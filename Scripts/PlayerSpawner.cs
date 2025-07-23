using Godot;
using System;

public partial class PlayerSpawner : Node3D
{
    [Export] PackedScene playerResource;
    int frameRate;
    Node player;
    public override void _Ready()
    {   
        player = playerResource.Instantiate();
        AddChild(player);
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
    public async void Reset()
    {
        player.QueueFree();
        frameRate = Engine.GetFramesDrawn();
        //i don't know why, but if we don't wait a frame here, then the player's children nodes don't reset properly
        await ToSignal(GetTree().CreateTimer(1/frameRate + .001F), SceneTreeTimer.SignalName.Timeout);
        GD.Print(1/frameRate + .001F);
        player = playerResource.Instantiate();
        AddChild(player);
    }
}
