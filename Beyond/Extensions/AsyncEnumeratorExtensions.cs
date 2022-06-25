// ReSharper disable once CheckNamespace

public static class AsyncEnumeratorExtensions
{
    public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(this IAsyncEnumerator<T> enumerator)
    {
        return enumerator;
    }

    public static async ValueTask<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)) list.Add(item);

        return list;
    }
}