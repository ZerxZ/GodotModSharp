using GodotModSharp.Interfaces;
using GodotModSharp.Loggers;

namespace GodotModSharp.Extensions;

public static class LoggerExtension
{
    public static void WriteLineInfo(this    ILogger logger, string text) => logger.WriteLine(LogKind.Info,    text);
    public static void WriteLineError(this   ILogger logger, string text) => logger.WriteLine(LogKind.Error,   text);
    public static void WriteLineWarning(this ILogger logger, string text) => logger.WriteLine(LogKind.Warning, text);
    public static void WriteLineDebug(this   ILogger logger, string text) => logger.WriteLine(LogKind.Debug,   text);
}