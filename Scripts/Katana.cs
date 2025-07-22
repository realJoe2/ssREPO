using Godot;
using System;

public partial class Katana : Weapon
{
    public override void _Ready()
    {
        state = WeaponState.SwitchTo;
    }
    public override void SwitchTo()
    {
        //play animations
        //switch to Idle state on animation finish. either do this via a signal or some other way.
        GD.Print("Switched to katana");

    }
    public override void Fire()
    {

    }
    public override void Idle()
    {

    }

}
