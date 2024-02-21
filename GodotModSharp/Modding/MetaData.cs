using GodotModSharp.Interfaces;

namespace GodotModSharp.Modding;

public class MetaData : IMetadata
{

    public string              Guid                     { get; set; } = "";
    public string              Version                  { get; set; } = "";
    public string              Author                   { get; set; } = "";
    public string              Name                     { get; set; } = "";
    public string              Description              { get; set; } = "";
    public string              Directory                { get; set; } = "";
    public IEnumerable<string> Dependencies             { get; set; } = new List<string>();
    public IEnumerable<string> DependenciesOptional     { get; set; } = new List<string>();
    public IEnumerable<string> DependenciesLoadBefore   { get; set; } = new List<string>();
    public IEnumerable<string> DependenciesLoadAfter    { get; set; } = new List<string>();
    public IEnumerable<string> DependenciesIncompatible { get; set; } = new List<string>();
    public IEnumerable<string> Tags                     { get; set; } = new List<string>();
}