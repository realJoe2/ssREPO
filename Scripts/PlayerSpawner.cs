using Godot;
using System;

public partial class PlayerSpawner : Node3D
{
    [Export] PackedScene playerResource;
    int frameRate;
    Node3D player;
    public override void _Ready()
    {   
        player = (Node3D) playerResource.Instantiate();
        AddChild(player);
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
    public void Reset()
    {
        player.GlobalPosition = GlobalPosition;
    }
}
