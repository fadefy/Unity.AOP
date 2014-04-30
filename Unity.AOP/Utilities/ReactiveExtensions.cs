using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Unity.AOP.Utilities
{
    public static class ReactiveExtensions
    {
        public static IObservable<T> RepeatWithInterval<T>(this Func<IObservable<T>> asyncAction, TimeSpan interval)
        {
            return Observable.Defer(asyncAction).DelaySubscription(interval).Repeat();
        }

        public static IObservable<T> RepeatWithInterval<T>(this Func<IObservable<T>> asyncAction, TimeSpan interval, int times)
        {
            return Observable.Defer(asyncAction).DelaySubscription(interval).Repeat(times);
        }
    }
}
