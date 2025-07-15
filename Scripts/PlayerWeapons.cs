using Godot;
using System;

public partial class PlayerWeapons : Node
{
	public override void _Ready()
	{

	}
}

public abstract partial class Weapon : Node
{
	public enum WeaponState
	{
		SwitchTo,
		Idle,
		Fire
	}
	public Timer switchTimer;
	public Timer attackTimer;
	public float secondsPerShot;
	public int damage;
	public WeaponState state;
	
	public abstract void SwitchTo();
	public abstract void Idle();
	public abstract void Fire();
	
	public void ChangeState(WeaponState s)
    {
        if(state == s)
            return;
        state = s;

        switch (state)
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
            default:
                GD.PushWarning("Tried to switch to a non-existent state.");
                break;
        }
    }

	public void DefineTimers()
	{
		switchTimer = (Timer) GetNode("switchTimer");
        if(switchTimer == null)
            GD.PushError("Switch timer not defined.");
        switchTimer.WaitTime = .33F;
        attackTimer = (Timer) GetNode("attackTimer");
        if(attackTimer == null)
            GD.PushError("Attack timer not defined.");
        attackTimer.WaitTime = secondsPerShot;
        switchTimer.OneShot = true;
        attackTimer.OneShot = true;
	}
}
