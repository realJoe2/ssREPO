using Godot;
using System;

public partial class Door : Node3D
{
    [Export] MeshInstance3D mesh;
    [Export] Direction direction;
    [Export] float lip;
    [Export(PropertyHint.Range, "0.0001,100,")] float moveSpeed = 10F;
    [Export] bool startLocked;
    [Export] bool startOpen;
    [Export] Vector3 turnAngles;

    [Signal] public delegate void DoorOpenedEventHandler();
    [Signal] public delegate void DoorClosedEventHandler();
    //[Signal] public delegate void DoorUnlockedEventHandler();
    bool locked;

    State state;
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back,
        Turn
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

        if (direction == Direction.Turn)
            startPosition = Rotation * 180/Mathf.Pi;
        else
            startPosition = Position;
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
            case Direction.Turn:
                endPosition = startPosition + turnAngles;
                endPosition *= Mathf.Pi / 180F;
                startPosition *= Mathf.Pi / 180F;
                break;
        }

        if(startOpen)
        {
            state = State.Open;
            if(direction == Direction.Turn)
                Rotation = endPosition;
            else
                Position = endPosition;
            return;
        }
        
        state = State.Closed;
        if(direction == Direction.Turn)
            Rotation = startPosition;
        else
            Position = startPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (state)
        {
            case State.Opening:
                if (direction == Direction.Turn) //dirty implementation of turning doors
                {
                    if (Rotation == endPosition)
                    {
                        state = State.Open;
                        EmitSignal(SignalName.DoorOpened);
                        break;
                    }
                    Rotation = Rotation.MoveToward(endPosition, moveSpeed);
                    break;
                }
                
                if (Position == endPosition)
                {
                    state = State.Open;
                    break;
                }
                Position = Position.MoveToward(endPosition, moveSpeed);
                break;
            case State.Closing:
                if (direction == Direction.Turn)
                {
                    if (Rotation == startPosition)
                    {
                        state = State.Closed;
                        EmitSignal(SignalName.DoorClosed);
                        break;
                    }
                    Rotation = Rotation.MoveToward(startPosition, moveSpeed);
                    break;
                }

                if (Position == startPosition)
                {
                    state = State.Closed;
                    break;
                }
                Position = Position.MoveToward(startPosition, moveSpeed);
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
    public void Toggle()
    {
        if(locked)
            return;
        
        if(state == State.Open || state == State.Opening)
        {
            state = State.Closing;
            return;
        }

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
