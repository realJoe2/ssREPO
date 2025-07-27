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


    Vector2 xyPosition;
	Vector2 playerXY;
	Vector2 direction;
	Vector3 newRotation;
    public void LookAtSmooth(Vector3 target, Node3D obj, float easing)
	{
		target.Y = obj.GlobalPosition.Y;
		xyPosition.X = obj.GlobalPosition.X;
        xyPosition.Y = obj.GlobalPosition.Z;
		playerXY.X = target.X;
        playerXY.Y = target.Z;
		direction = -(xyPosition - playerXY);
		newRotation = obj.Rotation;
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
    
    public bool CanSeeNode(Node3D node)
    {
        eyeSight.TargetPosition = eyeSight.ToLocal(node.GlobalPosition);
        eyeSight.ForceRaycastUpdate();
        if(eyeSight.IsColliding())
            return eyeSight.GetCollider() == node;
        return false;
    }

    public Vector3 GetForwardDirection(Node3D thing)
    {
        return (thing.GlobalPosition - (thing.GlobalPosition + thing.GlobalTransform.Basis.Z * -1).Normalized());
    }
    public bool ObjectIsFacing(Node3D thing, Node3D target)
    {
        return GetForwardDirection(thing).Dot((target.GlobalPosition - thing.GlobalPosition).Normalized()) > 0.7F;
    }

    public Vector3 GetNextPathPoint(Vector3 target)
    {
        navAgent.TargetPosition = target;
        return navAgent.GetNextPathPosition();
    }
}
