using Godot;
using System;

public partial class Shotgun : Weapon
{
    public override void _Ready()
    {
        secondsPerShot = .60F;
        damage = 1;
        state = WeaponState.SwitchTo;
        DefineTimers();
        SwitchTo();
    }

    public override void SwitchTo()
    {
        //play animations
        //switch to Idle state on animation finish. either do this via a signal or some other way.
    }
    public override void Fire()
    {

    }
    public override void Idle()
    {

    }

    public override void _Process(double delta)
    {
        //check for state conditions with another switch case
    }
}
