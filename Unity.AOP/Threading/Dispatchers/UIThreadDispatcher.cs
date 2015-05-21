using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Unity.AOP.Threading.Dispatchers
{
    public class UIThreadDispatcher : IInvocationDispatcher
    {
        private TimeSpan _timeout = TimeSpan.MaxValue;

        public TimeSpan Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public DispatcherPriority Priority { get; set; }

        public virtual T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult)
        {
            Contract.Requires<ArgumentNullException>(invocation != null, "invocation cannot be null.");
            try
            {
                return Application.Current.Dispatcher.Invoke(invocation, Priority, CancellationToken.None, Timeout);
            }
            catch (Exception)
            {
                return createDefaultResult();
            }
        }
    }
}
