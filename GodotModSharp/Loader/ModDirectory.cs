using System.Runtime.Loader;

namespace GodotModSharp.Loader;

public struct ModDirectory
{
    public string                      Directory        { get; private set; }
    public ModDirectoryKind            ModDirectoryKind { get; private set; }
    public AssemblyDependencyResolver? Resolver         { get; private set; }
    public ModDirectory(string directory)
    {
        Directory = directory;
        if (directory.StartsWith("res://") || directory.StartsWith("user://"))
        {
            ModDirectoryKind = ModDirectoryKind.ResourcePack;
            Resolver = default;
            return;
        }
        ModDirectoryKind = ModDirectoryKind.Local;
        Resolver = new AssemblyDependencyResolver(directory);
    }
}