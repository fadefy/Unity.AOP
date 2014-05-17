using System;
using System.Windows.Threading;

namespace Unity.AOP.Threading
{
    public interface IInvocationDispatcher
    {
        TimeSpan Timeout { get; set; }

        DispatcherPriority Priority { get; set; }

        T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult);
    }
}
