using Godot;
using System;

public partial class Shotgun : Weapon
{
    RayCast3D raycast;
    [Export] Resource bulletHoleResource;

    public override void _Ready()
    {
        state = WeaponState.SwitchTo;
        raycast = (RayCast3D) GetNode("RayCast3D");
        bulletDecal = QuickFetch.Fetch(bulletHoleResource);
    }

    public override void SwitchTo()
    {
        //play switchto animation
        GD.Print("Switched to shotgun");
        //switch to Idle state on animation finish. either do this via a signal or some other way.
        ChangeState(WeaponState.Idle);
        
    }

    PackedScene bulletDecal;
    Node3D collider;
    Node3D effects;
    public override void Fire()
    {
        //play fire animation

        raycast.Enabled = true;
        raycast.ForceRaycastUpdate();
        
        if(raycast.IsColliding())
        {
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
                    Vector3 rot = raycast.GetCollisionNormal() * 90F * (Mathf.Pi / 180F);
                    rot.Y = rot.Z;
                    rot.Z = rot.X;
                    rot.X = rot.Y;
                    rot.Y = 0F;
                    effects.Rotation = rot;
                }
            }
        }
        raycast.Enabled = false;

        //sync to animation player
        ChangeState(WeaponState.Idle);
    }
    public override void Idle()
    {
        //play idle animation
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Shoot"))
        {
            ChangeState(WeaponState.Fire);
        }
    }
}
