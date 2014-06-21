using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP
{
    public abstract class CallHandlerBase : ICallHandler
    {
        public int Order { get; set; }

        [Dependency]
        public IEventAggregator Aggregator { get; set; }

        [Dependency]
        public ILoggerFacade Logger { get; set; }

        [Dependency]
        public IUnityContainer Container { get; set; }

        public abstract IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext);

        protected void RaiseCompositeEvent<TPayload>(TPayload payload)
        {
            if (Aggregator != null)
                Aggregator.GetEvent<CompositePresentationEvent<TPayload>>().Publish(payload);
        }
    }
}
