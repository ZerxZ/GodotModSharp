using System.Reflection;
using System.Runtime.Loader;
using GodotModSharp.Loader;

namespace GodotModSharp.Interfaces;

public interface IMod
{
    IMetadata             Metadata   { get; }
    IEnumerable<Assembly> Assemblies { get; }
    IEnumerable<IPatch>   Patches    { get; }
    IEnumerable<IData>    Data       { get; }
    ModLoadContext        Context       { get; }
    IEnumerable<Type>          PluginsTypes  { get; }
    IEnumerable<IModPlugin>    Plugins       { get; }
    IEnumerable<IResourcePack> ResourcePacks { get; }
}