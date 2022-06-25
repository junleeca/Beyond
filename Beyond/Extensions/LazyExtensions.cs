// ReSharper disable once CheckNamespace

namespace Beyond.Extensions.LazyExtended;

public static class LazyExtensions
{
    public static void DisposeIfValueCreated<T>(this Lazy<T> lazy) where T : IDisposable
    {
        if (lazy != null && lazy.IsValueCreated)
            lazy.Value.Dispose();
    }
}