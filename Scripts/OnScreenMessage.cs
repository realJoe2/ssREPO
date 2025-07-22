using Godot;
using System;

public partial class OnScreenMessage : Control
{
	public override void _Ready()
	{
		Hide();
	}
	public void ShowMessage()
	{
		Show();
	}
	public void HideMessage()
	{
		Hide();
	}
	public void Reset()
	{
		Hide();
	}
}
