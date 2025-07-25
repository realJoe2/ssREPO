using Godot;
using System;

public partial class QuickFetch : Node
{
	//In order to keep the game running smoothly, resources should only be loaded during the "_Ready()" of whichever node instances them.
	public static PackedScene Fetch(string path)
	{
		if(ResourceLoader.Exists(path) == false)
		{
				GD.PushError("Path " + path + " invalid.");
				return null;
		}
		if(!ResourceLoader.HasCached(path))
			GD.Print("Loaded resource at path: " + path);
		return (PackedScene) ResourceLoader.Load(path, "", ResourceLoader.CacheMode.Reuse);
	}
}
