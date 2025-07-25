using Godot;
using System;

public partial class LevelChanger : Node
{
    GameManager gm;
    [Export] string levelPath;
    public override void _Ready()
    {
        gm = (GameManager) GetTree().GetNodesInGroup("Game Manager")[0];
        if(levelPath == null || levelPath == "")
            GD.PushWarning("Level path not set");
    }
    public void Activate()
    {
        gm.Call("ChangeLevel", levelPath);
    }
    public void SetPath(string s)
    {
        levelPath = s;
    }
}
