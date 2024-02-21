using System.Runtime.CompilerServices;
using Godot;
using GodotModSharp.Modding;

namespace GodotModSharp.Tests;

public partial class MainMenu : Node2D
{

	public override void _Ready()
	{
	 _=	ModManager.Instance;
	}
	// Called when the node enters the scene tree for the first time.

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}