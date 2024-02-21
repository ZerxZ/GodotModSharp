using Godot;

namespace GodotModSharp.Helper;

public class GodotHelper
{
    public static Window Root => ((SceneTree)Engine.GetMainLoop()).Root;
}