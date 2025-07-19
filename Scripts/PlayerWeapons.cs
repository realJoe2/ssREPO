using Godot;
using System;

public partial class PlayerWeapons : Node
{
    Weapon currentWeapon;
    Weapon previousWeapon;
    public override void _Ready()
    {
        currentWeapon = null;
        previousWeapon = null;
        //Equip("Shotgun");
    }

    public void EquipPrevious()
    {
        //GD.Print(previousWeapon);
        //GD.Print(currentWeapon);
        if (previousWeapon == null)
            return;
        if (currentWeapon != null)
            currentWeapon.Visible = false;

        Weapon temp = previousWeapon;
        previousWeapon = currentWeapon;
        currentWeapon = temp;

        currentWeapon.Visible = true;
        currentWeapon.state = Weapon.WeaponState.SwitchTo;
        currentWeapon.SwitchTo();
    }
    public void Equip(string weaponName)
    {
        Weapon nextWeapon = (Weapon)GetNodeOrNull(weaponName);
        if (nextWeapon == null && weaponName != "nil")
        {
            GD.Print("Couldn't find '" + weaponName + "'.");
            return;
        }
        if (currentWeapon == nextWeapon)
            return;

        //GD.Print("Equipped " + weaponName + ".");

        if (currentWeapon != null)
        {
            currentWeapon.Visible = false;
            previousWeapon = currentWeapon;
        }
        currentWeapon = nextWeapon;

        if (nextWeapon == null)
            return;
        currentWeapon.Visible = true;
        currentWeapon.state = Weapon.WeaponState.SwitchTo;
        currentWeapon.SwitchTo();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("EquipNothing"))
            Equip("nil");

        if (Input.IsActionJustPressed("EquipShotgun"))
            Equip("Shotgun");

        if (Input.IsActionJustPressed("EquipKatana"))
            Equip("Katana");
        
        if (Input.IsActionJustPressed("EquipPrevious"))
            EquipPrevious();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}

public abstract partial class Weapon : Node3D
{
	public enum WeaponState
	{
		SwitchTo,
		Idle,
		Fire
	}
	public Timer attackTimer;
	[Export] public float secondsPerShot;
	[Export] public int damage;
	public WeaponState state;
	
	public abstract void SwitchTo();
	public abstract void Idle();
	public abstract void Fire();

    public bool IsEquipped()
    {
        return (Weapon)GetParent().Call("GetCurrentWeapon") == this;
    }

	public void ChangeState(WeaponState s)
    {
        if (!IsEquipped())
            return;
        if (state == s)
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
        attackTimer = (Timer) GetNode("attackTimer");
        if(attackTimer == null)
            GD.PushError("Attack timer not defined.");
        attackTimer.WaitTime = secondsPerShot;
        attackTimer.OneShot = true;
	}
}
