using System;
using System.Windows.Threading;

namespace Unity.AOP.Threading.Dispatchers
{
    public class CurrentThreadDispatcher : IInvocationDispatcher
    {
        public DispatcherPriority Priority { get; set; }

        public TimeSpan Timeout { get; set; }

        public virtual T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult)
        {
            return invocation();
        }
    }
}
