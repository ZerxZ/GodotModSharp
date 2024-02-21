namespace GodotModSharp.Interfaces;

public interface IModManager
{
    IEnumerable<IMod> Mods { get; set; }
    IEnumerable<IMod> LoadMods();
}