using Godot;
using System;

public partial class PlayerSpawner : Node3D
{
    [Export] PackedScene playerResource;
    int frameRate;
    Node3D player;
    Vector3 startPosition;
    public override void _Ready()
    {   
        startPosition = GlobalPosition;
        player = (Node3D) playerResource.Instantiate();
        AddChild(player);
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        player.GlobalPosition = startPosition;
    }
    public void Reset()
    {
        player.Free();
        Node3D p = (Node3D) playerResource.Instantiate();
        AddChild(p);
        player = p;
        player.GlobalPosition = startPosition;
        GD.Print("Respawned player at the position " + startPosition + ".");
    }

    public void SetStartPosition(Vector3 sP)
    {
        startPosition = sP;
    }
}
