using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Unity.AOP.Caching;
using Unity.AOP.ExcptionHandling;
using Unity.AOP.HangingDetection;
using Unity.AOP.Logging;

namespace Unity.AOP
{
    public class UnityAopModule : IModule
    {
        public void Initialize()
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            if (container != null)
            {
                container.RegisterType<IInovcationStringBuilder, InvocationStringBuilder>(new ContainerControlledLifetimeManager());
                container.RegisterType<IIndentSizeProvider, ThreadIndentSizeProvider>(new ContainerControlledLifetimeManager());
                container.RegisterType<IHangingMonitor, ConcurrentHangingMonitor>(new ContainerControlledLifetimeManager());
                container.RegisterType<DetectHangingCallHandler>(new PerResolveLifetimeManager());
                container.RegisterType<LoggingCallHandler>(new PerResolveLifetimeManager());
                container.RegisterType<ExceptionCallHandler>(new PerResolveLifetimeManager());
                container.RegisterType<CacheCallHandler>(new PerResolveLifetimeManager());
            }
        }
    }
}
