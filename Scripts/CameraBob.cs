using Godot;
using System;

public partial class CameraBob : Camera3D
{
    [Export] CharacterBody3D playerNode;
    Node3D parent;
    [Export] float bobAmplitude;
    [Export] float bobFrequency;

    float velocitySpeed;
    Vector2 xyVelocity;
    Vector3 offset;

    public override void _Ready()
    {
        parent = (Node3D) GetParent();
    }
    public override void _Process(double delta)
    {
        offset = parent.Position;
        xyVelocity.X = playerNode.Velocity.X;
        xyVelocity.Y = playerNode.Velocity.Z;
        velocitySpeed = xyVelocity.Length();
        offset.Y += Mathf.Sin(Engine.GetPhysicsFrames() * bobFrequency) * velocitySpeed * bobAmplitude;
        Position = offset;
    }
}
