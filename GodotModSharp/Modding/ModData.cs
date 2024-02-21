using System.Reflection;
using System.Runtime.Loader;
using Godot;
using GodotModSharp.Attributes;
using GodotModSharp.Helper;
using GodotModSharp.Interfaces;
using GodotModSharp.PackedData;
using FileAccess = Godot.FileAccess;

namespace GodotModSharp.Modding;

public class ModData : IMod
{
    public ModData(IMetadata metadata)
    {
        Metadata = metadata;
        LoadAssemblies();
    }
#region Properties

    public IMetadata                  Metadata      { get; private set; }
    public IEnumerable<Assembly>      Assemblies    { get; private set; } = new List<Assembly>();
    public IEnumerable<IPatch>        Patches       { get; private set; } = new List<IPatch>();
    public IEnumerable<IResourcePack> ResourcePacks { get; private set; } = new List<IResourcePack>();
    public IEnumerable<IData>         Data          { get; private set; } = new List<IData>();
    public AssemblyLoadContext        Context       { get; private set; }
    public IEnumerable<Type>          PluginsTypes  { get; private set; } = new List<Type>();
    public IEnumerable<IModPlugin>    Plugins       { get; private set; } = new List<IModPlugin>();

#endregion

    #region Assembly Management

    public void LoadAssemblies()
    {
        Context = new AssemblyLoadContext(Metadata.Name, true);
        LoadDirectoryDll(Context);
        Assemblies = Context.Assemblies;
        var list = new List<Type>();
        foreach (var assembly in Assemblies)
        {
            var types = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Node)) &&
                               type.HasAttribute<ModPluginAttribute>() &&
                               type.HasInterface<IModPlugin>());
            list.AddRange(types);
        }
        PluginsTypes = list;
    }
    public void LoadDirectoryDll(AssemblyLoadContext context)
    {
        var directory = Metadata.Directory;
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }
        if (directory.StartsWith("res://") || directory.StartsWith("user://"))
        {
            LoadResourcePackDll(context, directory);
            return;
        }
        LoadLocalDll(context, directory);
    }
    public void LoadResourcePackDll(AssemblyLoadContext context, string directory)
    {
        using var dirAccess = DirAccess.Open(directory);
        var       files     = dirAccess.GetFiles("\\.dll$");

        foreach (var file in files)
        {
            var dllBytes = FileAccess.GetFileAsBytes(file);
            if (dllBytes is null)
            {
                continue;
            }
            var pdb       = file[..^4] + ".pdb";
            var dllStream = new MemoryStream(dllBytes);
            if (FileAccess.FileExists(pdb))
            {
                var pdbBytes  = FileAccess.GetFileAsBytes(pdb);
                var pdbStream = new MemoryStream(pdbBytes);
                context.LoadFromStream(dllStream, pdbStream);
                continue;
            }
            context.LoadFromStream(dllStream);
        }
    }
    public void LoadLocalDll(AssemblyLoadContext context, string directory)
    {
        var dlls = Directory.GetFiles(directory, "*.dll");
        foreach (var dll in dlls)
        {
            var dllStream = File.OpenRead(dll);
            var pdb       = dll[..^4] + ".pdb";
            if (File.Exists(pdb))
            {
                var pdbStream = File.OpenRead(pdb);
                context.LoadFromStream(dllStream, pdbStream);
                continue;
            }
            context.LoadFromStream(dllStream);
        }
    }
    public void ReloadAssemblies()
    {
        UnloadAssemblies();
        LoadAssemblies();
    }
    public void UnloadAssemblies()
    {
        foreach (var modPlugin in Plugins)
        {
            modPlugin.Unload();
        }
        Context.Unload();
        GC.Collect();
    }

  #endregion
#region Resource Pack Management

    public void LoadResourcePacks()
    {
        var directory = Metadata.Directory;
        ResourcePacks = Directory.GetFiles(directory, "*.*")
            .Select(filepath =>
            {
                return Path.GetExtension(filepath) switch
                {
                    ".zip" => (IResourcePack)new ZipData(filepath),
                    ".pck" => (IResourcePack)new PckData(filepath),
                    _      => null
                };

            }).Where(pack => pack != null)!;
    }

  #endregion
}