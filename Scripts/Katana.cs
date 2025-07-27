using Godot;
using System;

public partial class Katana : Weapon
{
    AudioStreamPlayer equipSound;
    public override void _Ready()
    {
        state = WeaponState.SwitchTo;
        equipSound = (AudioStreamPlayer) GetNode("Equip Sound");
    }
    public override void SwitchTo()
    {
        equipSound.Play();
        equipSound.PitchScale = (float) GD.Randfn(1.0, .05);
        //play animations
        //switch to Idle state on animation finish. either do this via a signal or some other way.

    }
    public override void Fire()
    {

    }
    public override void Idle()
    {

    }
}
