using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Framework;
using Unity.AOP.Mutation;
using Unity.AOP.Utilities;

namespace Unity.AOP.Caching
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CacheResultAttribute : PerMethodHandlerAttribute
    {
        public static string MutationScenario { get; set; } = "Cache";

        public override ICallHandler CreateHandler(IUnityContainer container, MethodImplementationInfo member)
        {
            var keyGenerator = GetKeyGenerator(container, member);
            var callHandler = container.Resolve<CacheCallHandler>(new ParameterOverride("keyGenerator", keyGenerator));

            return callHandler;
        }

        protected virtual IArgumentsCacheKeyGenerator GetKeyGenerator(IUnityContainer container, MethodImplementationInfo member)
        {
            var mutators = container.Resolve<IAggregatedMutator>();
            var argumentsTypes = member.ImplementationMethodInfo.GetParameters().Select(p => p.ParameterType).Where(t => !t.IsByRef).ToArray();
            var argumentsMutator = mutators.GenerateMutator<string>(argumentsTypes, MutationScenario);

            return new ArgumentsStringCacheKeyGenerator(argumentsMutator);
        }
    }
}
