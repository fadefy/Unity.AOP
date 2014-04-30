using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace Unity.AOP
{
    public abstract class CallHandlerBase : ICallHandler
    {
        [Dependency]
        public virtual int Order { get; set; }

        [Dependency]
        public IEventAggregator Aggregator { get; set; }

        [Dependency]
        public ILoggerFacade Logger { get; set; }

        public abstract IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext);

        protected virtual void Debug(string format, params object[] arguments)
        {
            if (Logger != null)
                Logger.Log(String.Format(format, arguments), Category.Debug, Priority.None);
        }

        protected virtual void Info(string format, params object[] arguments)
        {
            if (Logger != null)
                Logger.Log(String.Format(format, arguments), Category.Info, Priority.None);
        }

        protected virtual void Warning(string format, params object[] arguments)
        {
            if (Logger != null)
                Logger.Log(String.Format(format, arguments), Category.Warn, Priority.None);
        }

        protected virtual void Error(string format, params object[] arguments)
        {
            if (Logger != null)
                Logger.Log(String.Format(format, arguments), Category.Exception, Priority.None);
        }

        protected void RaiseCompositeEvent<TPayload>(TPayload payload)
        {
            if (Aggregator != null)
                Aggregator.GetEvent<CompositePresentationEvent<TPayload>>().Publish(payload);
        }
    }
}
