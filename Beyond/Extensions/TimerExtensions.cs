// ReSharper disable once CheckNamespace

using System.Timers;
using Timer = System.Timers.Timer;

namespace Beyond.Extensions.TimerExtended;

public static class TimerExtensions
{
    public static void Action(this Timer timer, double interval, Action action)
    {
        timer.Interval = interval;
        timer.Enabled = true;
        timer.Elapsed += (_, _) => action();
    }

    public static void Action(this Timer timer, double interval, Action<object, ElapsedEventArgs> action)
    {
        timer.Interval = interval;
        timer.Enabled = true;
        timer.Elapsed += (sender, e) =>
        {
            if (sender != null) action(sender, e);
        };
    }
}