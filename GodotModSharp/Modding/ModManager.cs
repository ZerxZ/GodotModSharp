using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Godot;
using GodotModSharp.Helper;
using GodotModSharp.Interfaces;
using GodotModSharp.Nodes;

namespace GodotModSharp.Modding;

public partial class ModManager : Autoload<ModManager>, IModManager
{
    [ModuleInitializer]
    public static void Initialize()
    {
        if (Engine.IsEditorHint())
        {
            return;
        }
        _ = Instance;
    }

    [Export] public string            Nodes;
    
    public          IEnumerable<IMod> Mods { get; set; } = new List<IMod>();
    public IEnumerable<IMod> LoadMods()
    {
        return new List<IMod>();
    }
    public string TEST()
    {
        return "";
    }
}