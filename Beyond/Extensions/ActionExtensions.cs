// ReSharper disable once CheckNamespace
// ReSharper disable UnusedMember.Global

namespace Beyond.Extensions.ActionExtended;

// ReSharper disable once UnusedType.Global
public static class ActionExtensions
{
    public static Action NeutralizeException(this Action action)
    {
        return () =>
        {
            try
            {
                action();
            }
            catch
            {
                // ignored
            }
        };
    }

    public static Action<object>? ToActionObject<T>(this Action<T>? actionT)
    {
        return actionT == null ? null : new Action<object>(o => actionT((T)o));
    }
    
    public static TimeSpan GetExecutionTime(this Action action)
    {
        var start = new Stopwatch();
        start.Start();
        action();
        start.Stop();
        return start.Elapsed;
    }
}