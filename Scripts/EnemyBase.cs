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

    public Node3D playerNode;
    RayCast3D eyeSight;
    public CharacterBody3D characterBody;
    NavigationAgent3D navAgent;

    public void Define()
    {
        var pl = GetTree().GetNodesInGroup("Player");
		playerNode = (CharacterBody3D) pl[0];
        characterBody = (CharacterBody3D) GetNode("../CharacterBody3D");
        eyeSight = (RayCast3D) GetNode("../CharacterBody3D/EyeSight");
        navAgent = (NavigationAgent3D) GetNode("../CharacterBody3D/NavigationAgent3D");
        if(playerNode == null)
            GD.PushError("Player node not found.");
        if(characterBody == null)
            GD.PushError("'CharacterBody3D' node not found. Ensure spelling.");
        if(eyeSight == null)
            GD.PushError("'EyeSight' (RayCast3D) node not found. Ensure spelling.");
        if(navAgent == null)
            GD.PushError("'NavigationAgent3D' node not found. Ensure spelling.");
    }

    Vector3 lookDirection;
    public void FacePlayer(Node3D model)
    {
        lookDirection = playerNode.GlobalPosition;
        lookDirection.Y = model.GlobalPosition.Y;
        model.LookAt(lookDirection);
        model.RotateY(Mathf.DegToRad(180F)); //flip around, otherwise the model points in the opposite direction
    }
    public void ChangeState(EnemyState s)
    {
        if(state == s)
            return;
        
        state = s;
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
    public bool CanSeePlayer()
    {
        eyeSight.TargetPosition = playerNode.GlobalPosition;
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
