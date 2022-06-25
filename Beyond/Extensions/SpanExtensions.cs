// ReSharper disable once CheckNamespace

namespace Beyond.Extensions.SpanExtended;

public static class SpanExtensions
{
    public static Span<T> AsSpan<T>(this List<T> list)
    {
        return CollectionsMarshal.AsSpan(list);
    }
}