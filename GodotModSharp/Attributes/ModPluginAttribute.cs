namespace GodotModSharp.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ModPluginAttribute : Attribute
{
    public ModPluginAttribute(string guid, string name, string version)
    {
        Guid = guid;
        Name = name;
        Version = version;
    }
    public string Guid { get; protected set; }
    public string Name { get; protected set; }
    public string Version { get; protected set; }
}