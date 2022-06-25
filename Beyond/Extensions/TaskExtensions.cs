// ReSharper disable once CheckNamespace

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using Beyond.Internals.Async;

namespace Beyond.Extensions.TaskExtended;

// ReSharper disable once UnusedType.Global
public static partial class TaskExtensions
{
   public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action,
        CancellationToken cancellationToken = default)
    {
        if (source is null) return source;

        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action.Invoke(item).ConfigureAwait(false);
        }

        return source;
    }

    public static async Task WaitAsync(this Task task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token))
            .ConfigureAwait(false);
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task.ConfigureAwait(false);
        }
        else
        {
            throw new TimeoutException();
        }
    }

    public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token))
            .ConfigureAwait(false);
        if (completedTask != task) throw new TimeoutException();
        timeoutCancellationTokenSource.Cancel();
        return await task.ConfigureAwait(false);
    }

    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, Task<TResult>> asyncSelector, CancellationToken cancellationToken = default)
    {
        if (source is null) return null;
        var result = new List<TResult>();
        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result.Add(await asyncSelector(item).ConfigureAwait(false));
        }

        return result;
    }

    public static void Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        for (var i = collection.Count - 1; i >= 0; i--)
        {
            var element = collection.ElementAt(i);
            if (predicate(element)) collection.Remove(element);
        }
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<IEnumerable<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<IOrderedEnumerable<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<IGrouping<TKey, TElement>>> AsEnumerable<TKey, TElement>(
        this Task<ILookup<TKey, TElement>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TElement>> AsEnumerable<TKey, TElement>(
        this Task<IGrouping<TKey, TElement>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<ICollection<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<IList<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<List<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<ISet<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<HashSet<TSource>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(
        this Task<TSource[]> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<KeyValuePair<TKey, TValue>>> AsEnumerable<TKey, TValue>(
        this Task<IDictionary<TKey, TValue>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<KeyValuePair<TKey, TValue>>> AsEnumerable<TKey, TValue>(
        this Task<Dictionary<TKey, TValue>> source)
    {
        return await source;
    }

    public static async Task<IEnumerable<TSource>> DequeuAsEnumerable<TSource>(
        this Task<Queue<TSource>> source)
    {
        return (await source).DequeueAsEnumerable();
    }

    public static Task<V> GroupJoin<T, U, K, V>(this Task<T> source, Task<U> inner, Func<T, K> outerKeySelector,
        Func<U, K> innerKeySelector, Func<T, Task<U>, V> resultSelector)
    {
        return source.TaskBind(t =>
            {
                return resultSelector(
                    t,
                    inner.Where(u =>
                        EqualityComparer<K>.Default.Equals(
                            outerKeySelector(t),
                            innerKeySelector(u)
                        )
                    )
                ).TaskUnit();
            }
        );
    }

    public static Task<V> Join<T, U, K, V>(this Task<T> source, Task<U> inner, Func<T, K> outerKeySelector,
        Func<U, K> innerKeySelector, Func<T, U, V> resultSelector)
    {
        Task.WaitAll(source, inner);

        return source.TaskBind(t =>
            {
                return inner.TaskBind(u =>
                    {
                        if (!EqualityComparer<K>.Default.Equals(outerKeySelector(t), innerKeySelector(u)))
                            throw new OperationCanceledException();

                        return resultSelector(t, u).TaskUnit();
                    }
                );
            }
        );
    }

    public static TResult RunSync<TResult>(this Task<TResult> task)
    {
        return AsyncHelper.RunSync(task);
    }

    public static TResult RunSync<TResult>(this Func<Task<TResult>> func)
    {
        return AsyncHelper.RunSync(func);
    }

    public static void RunSync(this Func<Task> func)
    {
        AsyncHelper.RunSync(func);
    }

    public static void RunSync(this Task task)
    {
        AsyncHelper.RunSync(task);
    }

    public static async void SafeFireAndForget(this Task @this, bool continueOnCapturedContext = true,
        Action<Exception> onException = null)
    {
        try
        {
            await @this.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception e) when (onException != null)
        {
            onException(e);
        }
    }

    public static Task<U> Select<T, U>(this Task<T> source, Func<T, U> selector)
    {
        return source.TaskBind(t => selector(t).TaskUnit());
    }

    public static Task<C> SelectMany<A, B, C>(this Task<A> monad, Func<A, Task<B>> function, Func<A, B, C> projection)
    {
        return monad.TaskBind(
            outer => function(outer).TaskBind(
                inner => projection(outer, inner).TaskUnit()));
    }

    public static Task<V> TaskBind<U, V>(this Task<U> m, Func<U, Task<V>> k)
    {
        return m.ContinueWith(m_ => k(m_.Result)).Unwrap();
    }

    public static Task<T> TaskUnit<T>(this T value)
    {
        return Task.Factory.StartNew(() => value);
    }

    public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToArray(), cancellationToken);
    }

    public static Task<T[]> ToArrayAsync<T>(this IEnumerable<T> query, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToArray(), cancellationToken);
    }

    public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> @this)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToArray();
    }

    public static Task<ICollection<T>> ToCollectionAsync<T>(this IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToCollection(), cancellationToken);
    }

    public static Task<ICollection<T>> ToCollectionAsync<T>(this IEnumerable<T> query,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToCollection(), cancellationToken);
    }

    public static async Task<IDictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector)
        where TKey : notnull
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToDictionary(keySelector);
    }

    public static async Task<IDictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        where TKey : notnull
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToDictionary(keySelector, keyComparer);
    }

    public static async Task<IDictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        where TKey : notnull
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToDictionary(keySelector, elementSelector);
    }

    public static async Task<IDictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> keyComparer)
        where TKey : notnull
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToDictionary(keySelector, elementSelector, keyComparer);
    }

    public static Task<HashSet<T>> ToHashSetAsync<T>(this IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.AsEnumerable().ToHashSet(), cancellationToken);
    }

    public static Task<HashSet<T>> ToHashSetAsync<T>(this IEnumerable<T> query,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToHashSet(), cancellationToken);
    }

    public static async Task<ISet<TSource>> ToHashSetAsync<TSource>(this Task<IEnumerable<TSource>> @this)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToHashSet();
    }

    public static async Task<ISet<TSource>> ToHashSetAsync<TSource>(this Task<IEnumerable<TSource>> @this,
        IEqualityComparer<TSource> comparer)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToHashSet(comparer);
    }

    public static Task<List<T>> ToListAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToList(), cancellationToken);
    }

    public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> query, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToList(), cancellationToken);
    }

    public static async Task<IList<T>> ToListAsync<T>(this Task<IEnumerable<T>> @this)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToList();
    }

    public static async Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this Task<IEnumerable<TSource>> @this,
        Func<TSource, TKey> keySelector)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToLookup(keySelector);
    }

    public static async Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this Task<IEnumerable<TSource>> @this,
        Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToLookup(keySelector, comparer);
    }

    public static async Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToLookup(keySelector, elementSelector);
    }

    public static async Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(
        this Task<IEnumerable<TSource>> @this, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer)
    {
        var source = await @this.ConfigureAwait(false);
        return source.ToLookup(keySelector, elementSelector, comparer);
    }

    public static Task<ObservableCollection<T>> ToObservableCollectionAsync<T>(this IEnumerable<T> enumerable,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() => enumerable.ToObservableCollection(), cancellationToken);
    }

    public static Task<IEnumerable<TEntity>> ToPagedAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex,
        int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToPaged(pageIndex, pageSize), cancellationToken);
    }

    public static Task<IEnumerable<TEntity>> ToPagedAsync<TEntity>(this IEnumerable<TEntity> query, int pageIndex,
        int pageSize, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => query.ToPaged(pageIndex, pageSize), cancellationToken);
    }

    public static Task<IReadOnlyCollection<TDestination>> ToReadOnlyCollectionAsync<TDestination>(
        this IEnumerable<TDestination> source, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => source.ToReadOnlyCollection(), cancellationToken);
    }

    public static Task<T> ToTask<T>(this T value)
    {
        return Task.FromResult(value);
    }

    public static Task ToTask(this Action action, CancellationToken cancellationToken = default)
    {
        return Task.Run(action, cancellationToken);
    }

    public static Task ToTask(this Func<Task> function, CancellationToken cancellationToken = default)
    {
        return Task.Run(function, cancellationToken);
    }

    public static Task<TResult> ToTask<TResult>(this Func<Task<TResult>> function,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(function, cancellationToken);
    }

    public static Task<TResult> ToTask<TResult>(this Func<TResult> function,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(function, cancellationToken);
    }

    public static T ToResult<T>(this Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }

    public static ConfiguredTaskAwaitable<T> ToTrueConfigureAwait<T>(this Task<T> task)
    {
        return task.ConfigureAwait(true);
    }

    public static ConfiguredTaskAwaitable<T> ToFalseConfigureAwait<T>(this Task<T> task)
    {
        return task.ConfigureAwait(false);
    }

    public static bool WaitCancellationRequested(this CancellationToken token, TimeSpan timeout)
    {
        return token.WaitHandle.WaitOne(timeout);
    }

    public static Task WhenAll(this IEnumerable<Task> tasks)
    {
        return Task.WhenAll(tasks.ToArray());
    }

    public static async Task<IEnumerable<TResult>> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        var result = await Task.WhenAll(tasks.ToArray());
        return result;
    }

    public static Task WhenAny(this IEnumerable<Task> tasks)
    {
        var enumeratedTasks = tasks as Task[] ?? tasks.ToArray();
        return Task.WhenAny(enumeratedTasks);
    }

    public static async Task<TResult> WhenAny<TResult>(this IEnumerable<Task<TResult>> tasks)
    {
        var enumeratedTasks = tasks as Task<TResult>[] ?? tasks.ToArray();
        var result = await await Task.WhenAny(enumeratedTasks);
        return result;
    }

    public static Task<T> Where<T>(this Task<T> source, Func<T, bool> predicate)
    {
        return source.TaskBind(t =>
        {
            if (!predicate(t))
                throw new OperationCanceledException();

            return t.TaskUnit();
        });
    }
}