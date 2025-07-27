using Godot;
using System;

public partial class Shotgun : Weapon
{
    RayCast3D raycast;
    [Export] string bulletHoleResourcePath;
    [Export] byte bulletsPerShot = 1;
    CharacterBody3D playerBody;
    [Export] float shotgunJumpForce;
    Timer dragTimer;
    float originalDrag;
    [Export] AnimationPlayer animation;
    [Export] float bulletSpreadAmount = 3F;

    [Export] AudioStreamPlayer shootSound;
    [Export] AudioStreamPlayer reloadSound;
    [Export] AudioStreamPlayer equipSound;

    public override void _Ready()
    {
        state = WeaponState.SwitchTo;
        raycast = (RayCast3D) GetNode("RayCast3D");
        bulletDecal = QuickFetch.Fetch(bulletHoleResourcePath);
        dragTimer = (Timer) GetNode("Drag Timer");
        playerBody = (CharacterBody3D) GetTree().GetNodesInGroup("Player")[0];
        originalDrag = (float) playerBody.Call("GetDrag");
    }

    public override void SwitchTo()
    {
        animation.Play("ShotgunSwitch");
        equipSound.PitchScale = (float) GD.Randfn(1.0, .05);
        equipSound.Play();
        //switch to Idle state on animation finish. either do this via a signal or some other way.
    }

    PackedScene bulletDecal;
    Node3D collider;
    Node3D effects;
    Vector3 shotgunJump;
    Vector3 bulletOffset;
    Vector3 rot;

    public override void Fire()
    {
        raycast.Enabled = true;
        for(int i = 0; i < bulletsPerShot; i++)
            FireShotgunShell();

        shootSound.PitchScale = (float) GD.Randfn(1.0, .02);
        reloadSound.PitchScale = (float) GD.Randfn(1.0, .02);
        shootSound.Play();
        reloadSound.Play();

        if(!playerBody.IsOnFloor()) //shotgun jumping! fuck yeah!!!
        {
            dragTimer.Start();
            shotgunJump = (raycast.GlobalPosition - (raycast.GlobalPosition + raycast.GlobalTransform.Basis.Z * -1)).Normalized();
            shotgunJump *= shotgunJumpForce;
            shotgunJump.Y = Mathf.Clamp(shotgunJump.Y, -30F, 30F);
            playerBody.Velocity += shotgunJump;
        }
        animation.Play("ShootShotgun");
        raycast.Enabled = false;
    }
    public override void Idle()
    {
        animation.Play("ShotgunSway");
        //play idle animation
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionPressed("Shoot") && state == WeaponState.Idle)
            ChangeState(WeaponState.Fire);
        if(playerBody.IsOnFloor())
            playerBody.Call("SetDrag", originalDrag);
        else
            playerBody.Call("SetDrag", originalDrag + dragTimer.TimeLeft * 10);
    }

    void OnAnimationFinished(string name)
    {
        if(name != "ShotgunSway")
            ChangeState(WeaponState.Idle);
    }

    void FireShotgunShell()
    {
        bulletOffset = Vector3.Forward * 100;
        bulletOffset.X += (float) GD.Randfn(0.0, bulletSpreadAmount);
        bulletOffset.Y += (float) GD.Randfn(0.0, bulletSpreadAmount);
        raycast.TargetPosition = bulletOffset;
        raycast.ForceRaycastUpdate();

        if(!raycast.IsColliding()) 
            return;

        collider = (Node3D) raycast.GetCollider();
        if(collider is Hitbox)
        {
            collider.Call("Hit", damage);
            //instance blood effects(?)
            //effects = (Node3D) bloodEffect.Instantiate();
        }
        else
        {
            effects = (Node3D) bulletDecal.Instantiate();
            collider.AddChild(effects);
            effects.GlobalPosition = raycast.GetCollisionPoint();
            if(raycast.GetCollisionNormal() != Vector3.Up || raycast.GetCollisionNormal() != Vector3.Down)
            {
                rot = raycast.GetCollisionNormal() * 90F * (Mathf.Pi / 180F);
                rot.Y = rot.Z;
                rot.Z = rot.X;
                rot.X = rot.Y;
                rot.Y = 0F;
                effects.Rotation = rot;
            }
        }
    }
}
