using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Threading;

namespace Unity.AOP.Threading.Dispatchers
{
    public class UIThreadAsyncDispatcher : IInvocationDispatcher
    {
        public DispatcherPriority Priority { get; set; }

        public TimeSpan Timeout { get; set; }

        public virtual T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult)
        {
            Contract.Requires<ArgumentNullException>(invocation != null, "invocation cannot be null.");
            Contract.Requires<ArgumentNullException>(createDefaultResult != null, "createDefaultResult canno be null.");
            Application.Current.Dispatcher.BeginInvoke(invocation, Priority);

            return createDefaultResult();
        }
    }
}
