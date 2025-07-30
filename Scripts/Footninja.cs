using Godot;
using System;

public partial class Footninja : EnemyBase
{
    [Export] int moveSpeed = 4;
    [Export] float attackDistance = 2F;
    [Export] byte damage;
    [Export] Area3D hurter;
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
        modelAnimator.Play("Run");
    }
    public override void Attack()
    {
        modelAnimator.Play("AttackSide");
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
            case EnemyState.Attack:
                movementVector = (playerNode.GlobalPosition - characterBody.GlobalPosition).Normalized();
                movementVector.Y = 0;
                characterBody.Call("AddForce", movementVector * moveSpeed * 0.80F);
                LookAtSmooth(playerNode.GlobalPosition, model, .1F);
                break;
        }
        
    }
    public void Hit()
    {
        hurter.SetDeferred("monitoring", true);
    }
    public void StopHit()
    {
        hurter.SetDeferred("monitoring", false);
    }
    void OnAnimationFinished(string animation)
    {
        if(animation == "AttackSide")
            ChangeState(EnemyState.PursuePlayer);
    }
}
