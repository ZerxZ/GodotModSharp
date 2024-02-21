namespace GodotModSharp.Interfaces;

public class Singleton<T>
{
    private static Lazy<T> _instance = new Lazy<T>(Activator.CreateInstance<T>);
    public static  T       Instance => _instance.Value;
    public static T CreateInstance(Func<T> func)
    {
        _instance = new Lazy<T>(func);
        return _instance.Value;
    }
}