using System.Reflection;
using System.Runtime.Loader;
using Godot;
using GodotModSharp.Attributes;
using GodotModSharp.Helper;
using GodotModSharp.Interfaces;
using FileAccess = Godot.FileAccess;

namespace GodotModSharp.Loader;

public class ModPluginLoadContext : AssemblyLoadContext
{
    protected List<ModDirectory> Directories { get; set; } = new List<ModDirectory>();

    public ModPluginLoadContext(string name, string directory) : base(name, true)
    {
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }
        Directories.Add(new ModDirectory(directory));
    }
    public ModPluginLoadContext(string name, IEnumerable<string> directory) : base(name, true)
    {
        Directories.AddRange(directory.Where(@path => !string.IsNullOrEmpty(@path)).Select(directories => new ModDirectory(directories)));
    }
    public ModPluginLoadContext(string name, IEnumerable<ModDirectory> directory) : base(name, true)
    {
        Directories.AddRange(directory);
    }
    public ModPluginLoadContext(string name, ModDirectory directory) : base(name, true)
    {
        Directories.Add(directory);
    }
    public void AddDirectory(string directory)
    {
        Directories.Add(new ModDirectory(directory));
    }
    public void AddDirectory(ModDirectory directory)
    {
        Directories.Add(directory);
    }
    public void AddDirectory(IEnumerable<string> directory)
    {
        Directories.AddRange(directory.Where(@path => !string.IsNullOrEmpty(@path)).Select(directories => new ModDirectory(directories)));
    }
    public void AddDirectory(IEnumerable<ModDirectory> directory)
    {
        Directories.AddRange(directory);
    }
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        foreach (var directory in Directories)
        {
            if (directory.ModDirectoryKind == ModDirectoryKind.ResourcePack)
            {
                continue;
            }
            var path = directory.Resolver!.ResolveAssemblyToPath(assemblyName);
            if (path != null)
            {
                return LoadFromAssemblyPath(path);
            }
        }
        return null;
    }

    public void LoadDirectoryDll()
    {
        foreach (var modDirectory in Directories)
        {
            switch (modDirectory.ModDirectoryKind)
            {

                case ModDirectoryKind.ResourcePack:
                    LoadResourcePackDll(modDirectory.Directory);
                    break;
                case ModDirectoryKind.Local:
                    LoadLocalDll(modDirectory.Directory);
                    break;
                default:
                    continue;
            }
        }
    }
    public void LoadResourcePackDll(string directory)
    {
        using var dirAccess = DirAccess.Open(directory);

        foreach (var file in dirAccess.GetFiles("\\.dll$"))
        {
            var dllBytes = FileAccess.GetFileAsBytes(file);
            if (dllBytes is null)
            {
                continue;
            }
            var pdb = file[..^4] + ".pdb";

            using var dllStream = new MemoryStream(dllBytes);
            if (FileAccess.FileExists(pdb))
            {
                var       pdbBytes  = FileAccess.GetFileAsBytes(pdb);
                using var pdbStream = new MemoryStream(pdbBytes);
                LoadFromStream(dllStream, pdbStream);
                continue;
            }
            LoadFromStream(dllStream);
        }
    }
    public void LoadLocalDll(string directory)
    {
        var dlls = Directory.GetFiles(directory, "*.dll");
        foreach (var dll in dlls)
        {
            using var dllStream = File.OpenRead(dll);
            var       pdb       = dll[..^4] + ".pdb";
            if (File.Exists(pdb))
            {
                using var pdbStream = File.OpenRead(pdb);
                LoadFromStream(dllStream, pdbStream);
                continue;
            }
            LoadFromStream(dllStream);
        }
    }
    public IEnumerable<Type> GetPluginsTypes()
    {
        var list = new List<Type>();
        foreach (var assembly in Assemblies)
        {
            var types = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Node)) &&
                               type.HasAttribute<ModPluginAttribute>() &&
                               type.HasInterface<IModPlugin>());
            list.AddRange(types);
        }
        return list;
    }
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        foreach (var directory in Directories)
        {
            if (directory.ModDirectoryKind == ModDirectoryKind.ResourcePack)
            {
                continue;
            }
            var path = directory.Resolver!.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (path != null)
            {
                return LoadUnmanagedDllFromPath(path);
            }
        }
        return IntPtr.Zero;
    }
}