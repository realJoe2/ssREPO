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
        
    }

    public override void SwitchTo()
    {

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
    public override void ChangeState(WeaponState s)
    {
        if(state == s)
            return;
        state = s;

        switch(state)
        {
            case WeaponState.SwitchTo:
                SwitchTo();
                break;
            case WeaponState.Idle:
                Idle();
                break;
            case WeaponState.Fire:
                Fire();
                break;
        }
    }
}
