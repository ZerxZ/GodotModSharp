using Godot;
using GodotModSharp.Interfaces;
using GodotModSharp.Utils;

namespace GodotModSharp.Nodes;

public partial class Autoload<TNode>:Node where TNode:Node
{
    private static TNode _instance;
    public static TNode Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = SingletonNode<TNode>.Instance;
            return _instance;
        }
    }
}