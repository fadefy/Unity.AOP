using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Caching;
using Unity.AOP.ExcptionHandling;
using Unity.AOP.HangingDetection;
using Unity.AOP.Logging;
using Unity.AOP.Mutation;
using Unity.AOP.Utilities;

namespace Unity.AOP
{
    public class UnityAopModule : IModule
    {
        public void Initialize()
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            if (container != null)
            {
                container.RegisterType<IInovcationStringBuilder, MethodInvocationStringBuilder>(new ContainerControlledLifetimeManager());
                container.RegisterType<IIndentSizeProvider, ThreadIndentSizeProvider>(new ContainerControlledLifetimeManager());
                container.RegisterType<IHangingMonitor, ConcurrentHangingMonitor>(new ContainerControlledLifetimeManager());
                container.RegisterType<IArgumentsCacheKeyGenerator, ArgumentsStringCacheKeyGenerator>(new ContainerControlledLifetimeManager());
                container.RegisterType<IAggregatedMutator, AggregatedMutator>(new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new AggregatedMutator().RegistBasicMutators()));
                container.RegisterType<DetectHangingCallHandler>(new PerResolveLifetimeManager());
                container.RegisterType<LoggingCallHandler>(new PerResolveLifetimeManager(),
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());
                container.RegisterType<ExceptionCallHandler>(new PerResolveLifetimeManager());
                container.RegisterType<CacheCallHandler>(new PerResolveLifetimeManager());
            }
        }
    }
}
