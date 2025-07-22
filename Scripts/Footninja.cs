using Godot;
using System;

public partial class Footninja : EnemyBase
{
    [Export] int moveSpeed = 1;
    Node3D model;
    AnimationPlayer modelAnimator;
    Timer idleTimer;

    public override void _Ready()
    {
        Define();
        model = (Node3D) GetNode("../ninja_model");
        modelAnimator = (AnimationPlayer) GetNode("../ninja_model/AnimationPlayer");
        idleTimer = (Timer) GetNode("../IdleBreak");
        Idle();
    }
    public override void Idle()
    {
        characterBody.Call("AddForce", Vector3.Zero);
        modelAnimator.Play("Idle");
        idleTimer.Start();

    }
    public override void PursuePlayer()
    {
        //play run animation
        modelAnimator.Play("Run");
    }
    public override void Attack()
    {
        if((GD.Randi() % 5) + 1 > 3)
            modelAnimator.Play("AttackSide");
        else
            modelAnimator.Play("Attack");
    }
    public override void Dead()
    {
        //play death effects
        
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
            case EnemyState.PursuePlayer:
                if(Engine.GetPhysicsFrames() % randomOffset == 0)
                    nextPath = GetNextPathPoint();
                
                if(characterBody.Velocity == Vector3.Zero)
                    modelAnimator.Pause();
                else
                    modelAnimator.Play("Run");

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
                characterBody.Call("AddForce", Vector3.Zero);
                if(modelAnimator.CurrentAnimationPosition > .74F)
                    ChangeState(EnemyState.Idle);
                LookAtSmooth(playerNode.GlobalPosition, model, .1F);
                break;

            default:
                break;
        }
    }

    public void HitPlayer()
    {
        if(distanceToPlayer <= 1.5F && state == EnemyState.Attack)
        {
            GD.Print("Hit!");
        }
    }
    void IdleTimeout()
    {
        if(distanceToPlayer < 1.5F && characterBody.IsOnFloor())
            ChangeState(EnemyState.Attack);
        else
            ChangeState(EnemyState.PursuePlayer);
    }
}
