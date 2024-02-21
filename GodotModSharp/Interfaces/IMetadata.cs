namespace GodotModSharp.Interfaces;

public interface IMetadata
{
    string              Guid                     { get; set; }
    string              Version                  { get; set; }
    string              Author                   { get; set; }
    string              Name                     { get; set; }
    string              Description              { get; set; }
    string              Directory                { get; set; }
    IEnumerable<string> Dependencies             { get; set; }
    IEnumerable<string> DependenciesOptional     { get; set; }
    IEnumerable<string> DependenciesLoadBefore   { get; set; }
    IEnumerable<string> DependenciesLoadAfter    { get; set; }
    IEnumerable<string> DependenciesIncompatible { get; set; }
    IEnumerable<string> Tags                     { get; set; }
}