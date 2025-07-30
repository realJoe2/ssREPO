using Godot;
using System;

public partial class Hurtbox : Area3D
{
    [Export] int damage = 1;
    void OnAreaEntered(Area3D a)
    {
        a.Call("Hit", damage);
    }
}
