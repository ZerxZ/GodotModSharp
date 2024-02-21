using Godot;
using GodotModSharp.Helper;

namespace GodotModSharp.Interfaces;

public class SingletonNode<TNode> where TNode : Node
{
    private static readonly Lazy<TNode> _instance = new(() =>
    {
        var root = GodotHelper.Root;
        var name = typeof(TNode).Name;
        foreach (var child in root.GetChildren())
        {
            if (child is TNode childInstance && childInstance.Name == name)
            {
                return childInstance;
            }
        }
        var instance = Activator.CreateInstance<TNode>();
        instance.Name = name;

        root.CallDeferred(Node.MethodName.AddChild, instance);
        return instance;
    });
    public static TNode Instance => _instance.Value;
}