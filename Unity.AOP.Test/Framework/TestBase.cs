using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.AOP.Framework;

namespace Unity.AOP.Test.Framework
{
    public class TestBase
    {
        public IUnityContainer Container { get; protected set; }

        public TestBase()
        {
            Container = new UnityContainer();
            Container.RegisterType<InjectionPolicy, PerMethodAttributeDrivenPolicy>("PerMethod");
            Container.AddNewExtension<Interception>();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            new UnityAopModule().Initialize();
            Container.RegisterType<ILoggerFacade, TraceLogger>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
        }
    }
}
