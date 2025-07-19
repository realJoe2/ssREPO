using Godot;
using System;

public abstract partial class EnemyBase : Node
{
    public enum EnemyState
    {
        Idle,
        PursuePlayer,
        Attack,
        Dead
    }
    public EnemyState state;

    public abstract void Idle();
    public abstract void PursuePlayer();
    public abstract void Attack();
    public abstract void Dead();

    public CharacterBody3D playerNode;
    RayCast3D eyeSight;
    public CharacterBody3D characterBody;
    public NavigationAgent3D navAgent;
    public uint randomOffset;

    public void Define()
    {
        randomOffset = (GD.Randi() % 3) + 10;
        playerNode = (CharacterBody3D) GetTree().GetNodesInGroup("Player")[0];
        characterBody = (CharacterBody3D) GetParent();
        eyeSight = (RayCast3D) GetNode("../EyeSight");
        navAgent = (NavigationAgent3D) GetNode("../NavigationAgent3D");
        if(playerNode == null)
            GD.PushError("Player node not found.");
        if(characterBody == null)
            GD.PushError("'CharacterBody3D' node not found. Ensure spelling..");
        if(eyeSight == null)
            GD.PushError("'EyeSight' (RayCast3D) node not found. Ensure spelling.");
        if(navAgent == null)
            GD.PushError("'NavigationAgent3D' node not found. Ensure spelling.");
    }

    public void LookAtSmooth(Vector3 n, Node3D obj, float easing)
	{
		n.Y = obj.GlobalPosition.Y;
		Vector2 xyPosition = new Vector2(obj.GlobalPosition.X, obj.GlobalPosition.Z);
		Vector2 playerXY = new Vector2(n.X, n.Z);
		Vector2 direction = -(xyPosition - playerXY);
		Vector3 newRotation = obj.Rotation;
		newRotation.Y = Mathf.LerpAngle(obj.Rotation.Y, Mathf.Atan2(direction.X, direction.Y), easing);
		obj.Rotation = newRotation;
	}
    public void ChangeState(EnemyState s)
    {
        if(state == s)
            return;
        state = s;
        //GD.Print(state);
        switch(state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.PursuePlayer:
                PursuePlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }
    public float GetDistanceToPlayer()
    {
        return characterBody.GlobalPosition.DistanceTo(playerNode.GlobalPosition);
    }
    public float GetHorizontalDistanceToPlayer()
    {
        Vector2 horizontal;
        horizontal.X = playerNode.GlobalPosition.X - characterBody.GlobalPosition.X;
        horizontal.Y = playerNode.GlobalPosition.Z - characterBody.GlobalPosition.Z;
        return horizontal.Length();
    }
    public float GetVerticalDistanceToPlayer()
    {
        return playerNode.GlobalPosition.Y - characterBody.GlobalPosition.Y;
    }
    public bool CanSeePlayer()
    {
        eyeSight.TargetPosition = eyeSight.ToLocal(playerNode.GlobalPosition);
        eyeSight.ForceRaycastUpdate();
        if(eyeSight.IsColliding())
            return eyeSight.GetCollider() == playerNode;
        return false;
    }
    public Vector3 GetNextPathPoint()
    {
        navAgent.TargetPosition = playerNode.GlobalPosition;
        return navAgent.GetNextPathPosition();
    }
}
