using Godot;
using System;

public partial class QuickFetch : Node
{
	//In order to keep the game running smoothly, resources should only be loaded during the "_Ready()" of whichever node instances them.
	public static PackedScene Fetch(Resource resource)
	{
		string path = resource.ResourcePath;
		if(ResourceLoader.Exists(path) == false)
		{
				GD.PushError("Path " + path + " invalid.");
				return null;
		}
		
		if(ResourceLoader.HasCached(path))
		{
			return (PackedScene) ResourceLoader.GetCachedRef(path);
		}
		return (PackedScene) ResourceLoader.Load(path);
	}
}
