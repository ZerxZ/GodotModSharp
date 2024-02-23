#if TOOLS
using Godot;

namespace GodotModSharp.Tests.addons.GodotModSharp;
[Tool]
public partial class GodotModSharp : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
	}
}
#endif
