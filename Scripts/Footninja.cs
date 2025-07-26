using Godot;
using System;

public partial class Footninja : EnemyBase
{
    [Export] int moveSpeed = 4;
    [Export] float attackDistance = 2F;
    Node3D model;
    AnimationPlayer modelAnimator;

    public override void _Ready()
    {
        Define();
        model = (Node3D) GetNode("../ninja_model");
        modelAnimator = (AnimationPlayer) GetNode("../ninja_model/AnimationPlayer");
        ChangeState(EnemyState.PursuePlayer);
    }
    public override void Idle()
    {

    }

    public override void PursuePlayer()
    {
        
    }
    public override void Attack()
    {

    }
    public override void Dead()
    {
        //play death effects
        GetParent().GetParent().Call("Defeated");
    }

    Vector3 nextPath = Vector3.Zero;
    Vector3 movementVector = Vector3.Zero;
    float distanceToPlayer;
    public override void _PhysicsProcess(double delta)
    {
        distanceToPlayer = GetDistanceToPlayer();
        //LookAtSmooth(playerNode.GlobalPosition, model, 1F);
        switch(state)
        {
            case EnemyState.PursuePlayer:
                if(Engine.GetPhysicsFrames() % randomOffset == 0)
                    nextPath = GetNextPathPoint(playerNode.GlobalPosition);
                
                movementVector = (nextPath - characterBody.GlobalPosition).Normalized();
                movementVector.Y = 0;
                
                if(distanceToPlayer <= 5F)
                    LookAtSmooth(playerNode.GlobalPosition, model, .1F);
                else
                    LookAtSmooth(nextPath, model, .1F);
                
                characterBody.Call("AddForce", movementVector * moveSpeed);

                //state changes
                if(distanceToPlayer <= attackDistance)
                    ChangeState(EnemyState.Attack);
                break;
        }
        
    }
}
