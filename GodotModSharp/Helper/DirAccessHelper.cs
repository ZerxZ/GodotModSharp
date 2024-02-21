using System.Text.RegularExpressions;
using Godot;
using JetBrains.Annotations;

namespace GodotModSharp.Helper;

public static class DirAccessHelper
{
    public static List<string> GetFiles(this DirAccess dirAccess, [RegexPattern] string expression)
    {
        var list  = new List<string>(128);
        var regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        list.AddRange(dirAccess.GetFiles().Where(filepath => regex.IsMatch(filepath)));
        var currentDirectory = dirAccess.GetDirectories().Where(dir => !string.IsNullOrEmpty(dir)).ToList();
        while (currentDirectory is { Count: > 0 })
        {
            var newDirectory = new List<string>(128);
            foreach (var directory in currentDirectory)
            {
                using var currentDirAccess = DirAccess.Open(directory);
                list.AddRange(dirAccess.GetFiles().Where(filepath => regex.IsMatch(filepath)));
                newDirectory.AddRange(currentDirAccess.GetDirectories().Where(dir => !string.IsNullOrEmpty(dir)));
            }
            currentDirectory = newDirectory;
        }
        return list;
    }
}