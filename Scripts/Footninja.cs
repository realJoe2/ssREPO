using Godot;
using System;

public partial class Footninja : EnemyBase
{
    Node3D model;
    AnimationPlayer modelAnimator;
    public override void _Ready()
    {
        Define();
        model = (Node3D) GetNode("../CharacterBody3D/ninja_model");
        modelAnimator = (AnimationPlayer) GetNode("../CharacterBody3D/ninja_model/AnimationPlayer");
        ChangeState(EnemyState.Idle);
    }
    public override void Idle()
    {
        modelAnimator.Play("Idle");
    }
    public override void PursuePlayer()
    {

    }
    public override void Attack()
    {
        
    }
    public override void Dead()
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        //check for conditions that change the state
        switch(state)
        {
            case EnemyState.Idle:
                FacePlayer(model);
                
                break;
        }
    }
}
