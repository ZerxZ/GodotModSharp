using System.Reflection;
using System.Runtime.Loader;
using Godot;
using GodotModSharp.Attributes;
using GodotModSharp.Helper;
using GodotModSharp.Interfaces;
using GodotModSharp.Loader;
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
    public ModLoadContext             Context       { get; private set; }
    public IEnumerable<Type>          PluginsTypes  { get; private set; } = new List<Type>();
    public IEnumerable<IModPlugin>    Plugins       { get; private set; } = new List<IModPlugin>();

#endregion

    #region Assembly Management

    public void LoadAssemblies()
    {
        Context = new ModLoadContext(Metadata.Name, Metadata.Directories);
        Context.LoadDirectoryDll();
        Assemblies = Context.Assemblies;
        PluginsTypes = Context.GetPluginsTypes();
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
        var directories = Metadata.Directories;
        foreach (var directory in directories)
        {
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

    }

  #endregion
}