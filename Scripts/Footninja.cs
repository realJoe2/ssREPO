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
        modelAnimator.Play("Idle");
        ChangeState(EnemyState.PursuePlayer);
    }
    public override void PursuePlayer()
    {
        //play run animation
    }
    public override void Attack()
    {
        
    }
    public override void Dead()
    {

    }

    Vector3 nextPath;
    Vector3 movementVector;
    public override void _PhysicsProcess(double delta)
    {
        //check for conditions that change the state
        switch(state)
        {
            case EnemyState.Idle:
                characterBody.Call("AddForce", Vector3.Zero);
                break;
            
            case EnemyState.PursuePlayer:
                if(Engine.GetPhysicsFrames() % randomOffset == 0)
                    nextPath = GetNextPathPoint();
                
                movementVector = (playerNode.Position - nextPath).Normalized();
                movementVector.Y = 0;
                
                if(characterBody.IsOnFloor())
                    GetParent().Call("AddForce", movementVector * moveSpeed * .4F);
                else
                    GetParent().Call("AddForce", movementVector * moveSpeed * .1F);
                break;
        }
    }
}
