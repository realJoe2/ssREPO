using Godot;
using System;

public partial class Footninja : EnemyBase
{
    [Export] int moveSpeed = 4;
    Node3D model;
    AnimationPlayer modelAnimator;

    public override void _Ready()
    {
        Define();
        model = (Node3D) GetNode("../ninja_model");
        modelAnimator = (AnimationPlayer) GetNode("../ninja_model/AnimationPlayer");
    }
    public override void Idle()
    {

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
        //play death effects
        GetParent().GetParent().Call("Defeated");
        if(GetParent() is CharacterBody3D)
            GetParent().QueueFree();
    }

    Vector3 nextPath;
    Vector3 movementVector;
    float distanceToPlayer;
    public override void _PhysicsProcess(double delta)
    {
        distanceToPlayer = GetDistanceToPlayer();
        //check for conditions that change the state
       
        
    }
}
