using Godot;
using System;

public partial class TimedFree : Decal
{
	public void Timeout()
	{
		//if(GetParent() is not StaticBody3D)
		GetParent().QueueFree();
	}
	public void Reset()
	{
		//if(GetParent() is not StaticBody3D)
		GetParent().QueueFree();
	}
}
