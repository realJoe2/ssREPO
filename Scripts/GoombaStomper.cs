using Godot;
using System;

public partial class GoombaStomper : Area3D
{
    [Export] int stompDamage;
    [Export] int stompBounce;
    Vector3 stompVector;
    void OnAreaEntered(Area3D a)
    {
        stompVector.X = (GD.Randi() % 10) - 5F;
        stompVector.Z = (GD.Randi() % 10) - 5F;
        stompVector.Y = stompBounce;
        a.Call("Hit", stompDamage);
        GetParent().Call("ResetY");
        GetParent().Call("AddForce", stompVector);
        //GD.Print("Stomped");
    }
}
