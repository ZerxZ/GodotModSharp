using Godot;
using GodotModSharp.Interfaces;

namespace GodotModSharp.PackedData;

public class ZipData : IResourcePack
{
    public ZipData(string filepath, bool replaceFiles = true, int offset = 0)
    {
        FilePath = filepath;
        ReplaceFiles = replaceFiles;
        Offset = offset;
    }

    public string FilePath     { get; set; }
    public bool   ReplaceFiles { get; set; }
    public int    Offset       { get; set; }

    public void Load()
    {
        ProjectSettings.LoadResourcePack(FilePath, ReplaceFiles, Offset);
    }
}