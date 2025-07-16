using Godot;
using System;

public partial class Footninja : EnemyBase
{
    [Export] int moveSpeed = 0;
    Node3D model;
    AnimationPlayer modelAnimator;

    public override void _Ready()
    {
        Define();
        model = (Node3D) GetNode("../ninja_model");
        modelAnimator = (AnimationPlayer) GetNode("../ninja_model/AnimationPlayer");
        Idle();
    }
    public override void Idle()
    {
        characterBody.Call("AddForce", Vector3.Zero);
        modelAnimator.Play("Idle");
    }
    public override void PursuePlayer()
    {
        //play run animation
        modelAnimator.Play("Run");
    }
    public override void Attack()
    {
        characterBody.Call("AddForce", Vector3.Zero);
        modelAnimator.Play("Attack");
    }
    public override void Dead()
    {
        GetParent().GetParent().Call("Defeated");
    }

    Vector3 nextPath;
    Vector3 movementVector;
    float distanceToPlayer;
    public override void _PhysicsProcess(double delta)
    {
        distanceToPlayer = GetDistanceToPlayer();
        //check for conditions that change the state
        switch(state)
        {
            case EnemyState.Idle:
                if(distanceToPlayer < 200F)
                    ChangeState(EnemyState.PursuePlayer);
                break;
            
            case EnemyState.PursuePlayer:
                if(Engine.GetPhysicsFrames() % randomOffset == 0)
                    nextPath = GetNextPathPoint();
                
                movementVector = (nextPath - characterBody.GlobalPosition).Normalized();
                movementVector.Y = 0;
                
                if(distanceToPlayer > 5F)
                    LookAtSmooth(nextPath, model, .5F);
                else
                    LookAtSmooth(playerNode.GlobalPosition, model, .5F);
                
                if (characterBody.IsOnFloor())
                    GetParent().Call("AddForce", movementVector * moveSpeed * .4F);
                else
                    GetParent().Call("AddForce", movementVector * moveSpeed * .1F);
                
                if(distanceToPlayer < 1.5F && characterBody.IsOnFloor())
                    ChangeState(EnemyState.Attack);
                if(distanceToPlayer >= 200F)
                    ChangeState(EnemyState.Idle);
                break;

            case EnemyState.Attack:
                if(distanceToPlayer <= 1.5F && characterBody.IsOnFloor())
                    Attack();
                else if(modelAnimator.CurrentAnimationPosition > .83F)
                    ChangeState(EnemyState.PursuePlayer);
                LookAtSmooth(playerNode.GlobalPosition, model, .1F);
                break;
        }
    }

    public void HitPlayer()
    {
        if(distanceToPlayer <= 1.5F && state == EnemyState.Attack)
        {
            GD.Print("Hit!");
            Vector3 hitForce = characterBody.GlobalPosition.DirectionTo(playerNode.GlobalPosition) * 20;
            hitForce.Y = 1;
            playerNode.Call("AddForce", hitForce);
            characterBody.Call("AddForce", -hitForce);
        }
    }
}
