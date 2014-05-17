using System;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Threading.Dispatchers;

namespace Unity.AOP.Threading
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class ThreadDispatchingAttribute : GenericHandlerAttribute
    {
        public ThreadDispatchingAttribute()
            : base(typeof(DispatchingCallHandler))
        {
        }

        public bool Async { get; set; }

        public bool IsSequenceCritical { get; set; }

        public TimeSpan Timeout { get; set; }

        public DispatcherPriority Priority { get; set; }

        public ThreadType TargetThreadType { get; set; }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new DispatchingCallHandler(CreateInvocationDispather());
        }

        protected virtual IInvocationDispatcher CreateInvocationDispather()
        {
            if (TargetThreadType == ThreadType.Background)
            {
                if (Async)
                    return new BackgroundThreadAsyncDispatcher() { Priority = Priority, Timeout = Timeout };
                else
                    return new BackgroundThreadDispatcher() { Priority = Priority, Timeout = Timeout };
            }
            else if (TargetThreadType == ThreadType.Foreground)
            {
                if (Async)
                    return new UIThreadAsyncDispatcher() { Priority = Priority, Timeout = Timeout };
                else
                    return new UIThreadDispatcher() { Priority = Priority, Timeout = Timeout };
            }
            else if (TargetThreadType == ThreadType.Current)
            {
                return new CurrentThreadDispatcher() { Priority = Priority, Timeout = Timeout };
            }
            else
                throw new NotSupportedException();
        }
    }
}
