using Godot;
using System;
using System.Threading.Tasks;

public partial class TriggerHandler : Node
{
	public void Run(String[] commands)
	{
		if(commands == null)
			return;
		for(int i = 0; i < commands.Length; i++)
		{
			if(commands[i] != null && commands[i] != "")
			{
				//GD.Print("Scheduling: '" + commands[i] + "'.");
				RunEach(commands[i].Split(','));
			}
		}
	}
	
	private async void RunEach(String[] s)
	{
		if(s.Length != 3)
		{
			GD.PushError("Faulty arguments for trigger.");
			return;
		}
		Node node = GetNode("../../" + s[0]);
		if(node == null)
		{
			GD.PushError(s[0] + " not found. Correct spelling or ensure that the input and output nodes have the same parent.");
			return;
		}
		String functionToCall = s[1];
		float delay = Int32.Parse(s[2])/10;
		if(delay > 0F)
			await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		GD.Print("Ran command '" + functionToCall + "' in " + s[0]+ " after activating trigger from " + GetParent() + " with a delay of " + delay + " second(s).");
		node.Call(functionToCall);
	}
}
