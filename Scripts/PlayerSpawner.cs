using Godot;
using System;

public partial class PlayerSpawner : Node3D
{
    [Export] PackedScene playerResource;
    Node player;
    public override void _Ready()
    {
        player = playerResource.Instantiate();
        AddChild(player);
    }
    public void Reset()
    {
        player.QueueFree();
        player = playerResource.Instantiate();
        //check for checkpoint logic???
        AddChild(player);
    }
}
