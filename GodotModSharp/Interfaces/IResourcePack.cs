namespace GodotModSharp.Interfaces;

public interface IResourcePack
{
    string FilePath     { get; set;}
    bool   ReplaceFiles { get; set;}
    int    Offset       { get; set;}
    void   Load();
}