using Godot;
using System;

public partial class Door : Node3D
{
    [Export] MeshInstance3D mesh;
    [Export] Direction direction;
    [Export] float lip;
    [Export(PropertyHint.Range, "0.0001,100,")] float moveSpeed;
    [Export] bool startLocked;
    [Export] bool startOpen;
    bool locked;

    State state;
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    }
    enum State
    {
        Open,
        Closed,
        Opening,
        Closing
    }

    Vector3 startPosition, endPosition, meshSize;
    public override void _Ready()
    {
        if (mesh == null)
        {
            GD.PushError("Mesh is null, culling Door.");
            QueueFree();
        }
        moveSpeed = moveSpeed / 100;
        locked = startLocked;
        meshSize = mesh.GetAabb().Size;
        startPosition = GlobalPosition;
        endPosition = startPosition;

        switch (direction)
        {
            case Direction.Up:
                endPosition.Y = startPosition.Y + meshSize.Y - lip;
                break;
            case Direction.Down:
                endPosition.Y = startPosition.Y - meshSize.Y + lip;
                break;
            case Direction.Right:
                endPosition.X = startPosition.X + meshSize.X - lip;
                break;
            case Direction.Left:
                endPosition.X = startPosition.X - meshSize.X + lip;
                break;
            case Direction.Forward:
                endPosition.Z = startPosition.Z + meshSize.X - lip;
                break;
            case Direction.Back:
                endPosition.Z = startPosition.Z - meshSize.X + lip;
                break;
        }
        
        if (!startOpen)
        {
            state = State.Closed;
            return;
        }
        state = State.Open;
        GlobalPosition = endPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (state)
        {
            case State.Opening:
                if (GlobalPosition == endPosition)
                {
                    state = State.Open;
                    break;
                }
                GlobalPosition = GlobalPosition.MoveToward(endPosition, moveSpeed);
                break;
            case State.Closing:
                if (GlobalPosition == startPosition)
                {
                    state = State.Closed;
                    break;
                }
                GlobalPosition = GlobalPosition.MoveToward(startPosition, moveSpeed);
                break;
        }
    }

    public void Open()
    {
        if (state == State.Opening || state == State.Open)
            return;
        if (locked)
            return;
        state = State.Opening;
        
    }
    public void Close()
    {
        if (state == State.Closing || state == State.Closed)
            return;
        if (locked)
            return;
        state = State.Closing;
    }
    public void Lock()
    {
        locked = true;
    }
    public void Unlock()
    {
        locked = false;
    }
}
