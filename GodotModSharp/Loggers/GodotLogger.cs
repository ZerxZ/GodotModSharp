using Godot;
using GodotModSharp.Interfaces;

namespace GodotModSharp.Loggers;

public class GodotLogger : ILogger
{
    public static ILogger Default { get; } = new GodotLogger();
    public GodotLogger()
    {
    }
    public GodotLogger(string prefix)
    {
        Prefix = $"[{prefix}]:";
        _hasPrefix = true;
    }
    private readonly bool   _hasPrefix = false;
    public           string Prefix { get; set; } = string.Empty;

    private static readonly Dictionary<LogKind, string> ColorScheme =
        new Dictionary<LogKind, string>
        {
            { LogKind.Info, nameof(Colors.Gray).ToLower() },
            { LogKind.Debug, nameof(Colors.Green).ToLower() },
            { LogKind.Warning, nameof(Colors.Yellow).ToLower() },
            { LogKind.Error, nameof(Colors.Red).ToLower() },
        };

    public void WriteLine(LogKind logKind, string message) => PrintRich(logKind, GD.PrintRich, message);
    public void PrintRich(LogKind logKind, Action<string> writer, string message)
    {
        var text = _hasPrefix ? $"{Prefix}{message}" : message;
        switch (logKind)
        {
            case LogKind.Error:
                if (_hasPrefix)
                {
                    writer($"[color={ColorScheme[logKind]}]{text}[/color]");
                }

                GD.PushError(message);
                break;
            case LogKind.Warning:
                if (_hasPrefix)
                {
                    writer($"[color={ColorScheme[logKind]}]{text}[/color]");
                }
                GD.PushWarning(logKind);
                break;
            case LogKind.Info:
            case LogKind.Debug:
            default:
                writer($"[color={ColorScheme[logKind]}]{text}[/color]");
                break;
        }
    }
    public void WriteLine() => GD.Print();
    public void Flush()
    {

    }
}