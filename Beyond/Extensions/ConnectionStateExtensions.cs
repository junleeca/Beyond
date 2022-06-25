// ReSharper disable once CheckNamespace

namespace Beyond.Extensions.ConnectionStateExtended;

public static class ConnectionStateExtensions
{
    public static bool In(this ConnectionState @this, params ConnectionState[] values)
    {
        return values.IndexOf(@this) != -1;
    }

    public static bool NotIn(this ConnectionState @this, params ConnectionState[] values)
    {
        return values.IndexOf(@this) == -1;
    }
}