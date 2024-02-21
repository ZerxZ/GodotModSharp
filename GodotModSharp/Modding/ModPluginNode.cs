using System.Runtime.CompilerServices;
using Godot;
using GodotModSharp.Interfaces;
using GodotModSharp.Loggers;

namespace GodotModSharp.Modding;

public partial class ModPluginNode: Node, IModPlugin
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void Initialize()
    {
        if (Engine.IsEditorHint())
        {
            return;
        }
        _ = Instance;
    }
 

    private static ModPluginNode _instance;
    public static ModPluginNode Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            GD.Print("Instance of ModPluginNode created");
            _instance = SingletonNode<ModPluginNode>.Instance;
            return _instance;
        }
    }
    public void InitMod(IMod mod, ILogger logger)
    {
        Mod   = mod;
        Logger = logger;
    }

    public IMod    Mod    { get; set; } = default!;
    public ILogger Logger { get; set; } = GodotLogger.Default;
    public void LogInfo(string message) => Logger.WriteLine(LogKind.Info, message);
    public void LogWarning(string message) => Logger.WriteLine(LogKind.Warning, message);
    public void LogError(string message) => Logger.WriteLine(LogKind.Error, message);
    public void LogDebug(string message) => Logger.WriteLine(LogKind.Debug, message);
    public virtual void    Unload() => throw new NotImplementedException();
    public virtual void    Load()   => throw new NotImplementedException();
}