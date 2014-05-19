using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Framework;
using Unity.AOP.Mutation;
using Unity.AOP.Utilities;

namespace Unity.AOP.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class LoggingInvocationAttribute : PerMethodHandlerAttribute
    {
        public int IndentSize { get; set; }

        public bool IncludesArguments { get; set; }

        public static string MutationScenario { get; set; } = "Logging";

        public override ICallHandler CreateHandler(IUnityContainer container, MethodImplementationInfo member)
        {
            var loggingCallHandler = CreateCallHandler(container);
            loggingCallHandler.Order = Order;
            loggingCallHandler.IncludesArguments = IncludesArguments;
            loggingCallHandler.IndentSize = IndentSize;
            loggingCallHandler.Builder = CreateStringBuilder(member.ImplementationMethodInfo, container);

            return loggingCallHandler;
        }

        protected virtual IInovcationStringBuilder CreateStringBuilder(MethodInfo methodInfo, IUnityContainer container)
        {
            var mutator = GetMutator(container);
            var arguments = methodInfo.GetParameters();
            var ignoredArgumentIndexes = (from argumentWithIndex in arguments.Select((p, i) => new { Parameter = p, Index = i })
                                          where argumentWithIndex.Parameter.GetCustomAttributes<ExcludeFromLogAttribute>().Any()
                                          select argumentWithIndex.Index).ToList();
            var argumentTypes = arguments.Exclude(ignoredArgumentIndexes).Select(p => p.ParameterType).ToArray();
            var argumentsMutator = mutator.GenerateMutator<string>(argumentTypes, MutationScenario);
            var returnMutator = mutator.GenerateMutator<string>(methodInfo.ReturnType, MutationScenario);

            return new MethodInvocationStringBuilder(ignoredArgumentIndexes, argumentsMutator, returnMutator);
        }

        protected virtual LoggingCallHandler CreateCallHandler(IUnityContainer container)
        {
            return container.Resolve<LoggingCallHandler>();
        }

        protected virtual IAggregatedMutator GetMutator(IUnityContainer container)
        {
            return container.Resolve<IAggregatedMutator>();
        }
    }
}
