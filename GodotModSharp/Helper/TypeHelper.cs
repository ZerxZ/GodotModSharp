namespace GodotModSharp.Helper;

public static class TypeHelper
{
    public static bool HasInterface<T>(this Type type)
    {
        return type.GetInterfaces().Contains(typeof(T));
    }
    public static bool HasAttribute<T>(this Type type)where T:Attribute
    {
        return type.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}