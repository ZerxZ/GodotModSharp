using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;
using GodotModSharp.Modding;

namespace GodotModSharp.Tests;

public partial class MainMenu : Node2D
{
    public IEnumerable<string> Split(ReadOnlySpan<char> str, char separator)
    {
        var index  = 0;
        var length = str.Length;
        var result = new List<string>(64);
        while (index < length)
        {
            var slice     = str[index..];
            var nextIndex = slice.IndexOf(separator);
            if (nextIndex == -1)
            {
                result.Add(slice.ToString());
                break;
            }
            result.Add(str[index..(index + nextIndex)].ToString());
            index += nextIndex + 1;
        }
        return result;
    }

    public override void _Ready()
    {
        foreach (var str in Split(@"C:\Users\non78\.nuget\packages\gdunit4.api\4.2.0-rc.202402212033", Path.DirectorySeparatorChar))
        {
            GD.Print(str);
        }

    }
    // Called when the node enters the scene tree for the first time.

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}