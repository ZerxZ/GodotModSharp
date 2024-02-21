using GodotModSharp.Loggers;

namespace GodotModSharp.Interfaces;

public interface ILogger
{
    void WriteLine(LogKind logKind, string message);
    void WriteLine();
    void Flush();
}