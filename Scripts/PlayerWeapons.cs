using Godot;
using System;

public partial class PlayerWeapons : RayCast3D
{
	[Export] AnimationPlayer shotgunModel;
	[Export] AnimationPlayer katanaModel;
	[Export] AnimationPlayer playerArmsModel;
	Timer switchTimer;
	Timer attackTimer;

	enum State : byte
	{
		DEFAULT,
		SHOTGUN_SWITCH,
		SHOTGUN,
		SHOTGUN_SHOOT,
		KATANA_SWITCH,
		KATANA,
		KATANA_SWING
	}
	State weaponState = State.DEFAULT;

	public override void _Ready()
	{
		switchTimer = (Timer) GetNode("SwitchTimer");
		attackTimer = (Timer) GetNode("AttackTimer");
		ChangeState(State.SHOTGUN_SWITCH);
	}

	public override void _Process(double delta)
	{
		//if the player switches weapons in the middle of a swing/shoot animation, then that animation should stop prematurely and the corresponding switch animation should play
		switch(weaponState)
		{
			case State.SHOTGUN:
				if(Input.IsActionPressed("Shoot"))
					ChangeState(State.SHOTGUN_SHOOT);
				break;
		}
	}

	void ChangeState(State newState)
	{
		if(newState == weaponState)
			return;

		weaponState = newState;
		//GD.Print(weaponState);
		switch(weaponState)
		{
			case State.SHOTGUN_SWITCH:
				switchTimer.Start();
				//playerArmsModel.Play("ShotgunSwitch");
				shotgunModel.Play("ShotgunSwitch");
				
				break;

			case State.SHOTGUN:
				//playerArmsModel.Play("ShotgunSway");
				shotgunModel.Play("ShotgunSway");
				break;

			case State.SHOTGUN_SHOOT:
				attackTimer.Start();
				//fire a bullet and add knockback to player
				Enabled = true;
				ForceRaycastUpdate();
				if(IsColliding())
				{
					var collider = GetCollider();
					if(collider is Area3D)
					{
						collider.Call("Hit", 1);
						//instance blood effects maybe?
					}
					else
					{
						//collider is an environment object, so instance bullet holes
						
					}
				}
				Enabled = false;

				//playerArmsModel.Play("ShotgunShoot");
				shotgunModel.Play("ShotgunShoot");

				break;

			case State.KATANA_SWITCH:
				switchTimer.Start();
				//playerArmsModel.Play("KatanaSwitch");
				katanaModel.Play("KatanaSwitch");
				
				break;

			case State.KATANA:
				//playerArmsModel.Play("KatanaSway");
				katanaModel.Play("KatanaSway");
				break;
				
			case State.KATANA_SWING:
				attackTimer.Start();
				//instance hitbox after a delay of .33 seconds

				//playerArmsModel.Play("KatanaSwing");
				katanaModel.Play("KatanaSwing");
				
				break;
		}
	}

	void OnSwitchTimerTimeout()
	{
		if(weaponState == State.SHOTGUN_SWITCH)
			ChangeState(State.SHOTGUN);
		else if(weaponState == State.KATANA_SWITCH)
			ChangeState(State.KATANA);
	}
	void OnAttackTimerTimeout()
	{
		if(weaponState == State.SHOTGUN_SHOOT)
			ChangeState(State.SHOTGUN);
		else if(weaponState == State.KATANA_SWING)
			ChangeState(State.KATANA);
	}
}
