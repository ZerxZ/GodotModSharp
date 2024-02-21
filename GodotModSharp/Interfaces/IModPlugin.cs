namespace GodotModSharp.Interfaces;

public interface IModPlugin
{
    IMod    Mod    { get; set; }
    ILogger Logger { get; set; }
    void    Unload();
    void    Load();
}