using System.Reflection;
using System.Runtime.Loader;

namespace GodotModSharp.Interfaces;

public interface IMod
{
    IMetadata                  Metadata      { get; }
    IEnumerable<Assembly>      Assemblies    { get; }
    IEnumerable<IPatch>        Patches       { get; }
    IEnumerable<IData>         Data          { get; }
    AssemblyLoadContext        Context       { get; }
    IEnumerable<Type>          PluginsTypes  { get; }
    IEnumerable<IModPlugin>    Plugins       { get; }
    IEnumerable<IResourcePack> ResourcePacks { get; }
}