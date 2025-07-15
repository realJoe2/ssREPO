using Godot;
using System;

public partial class Footninja : EnemyBase
{
    public override void _Ready()
    {
        Define();
        Idle();
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

    }
    public override void _PhysicsProcess(double delta)
    {
        //check for conditions that change the state
    }
}
