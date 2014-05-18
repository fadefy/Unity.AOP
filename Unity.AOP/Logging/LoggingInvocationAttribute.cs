using System;
using System.Linq;
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
            var loggingCallHandler = container.Resolve<LoggingCallHandler>();
            loggingCallHandler.Order = Order;
            loggingCallHandler.IncludesArguments = IncludesArguments;
            loggingCallHandler.IndentSize = IndentSize;
            var mutator = container.Resolve<IAggregatedMutator>();
            var argumentTypes = member.ImplementationMethodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            var argumentsMutator = mutator.GenerateMutator<string>(argumentTypes, MutationScenario);
            var returnMutator = mutator.GenerateMutator<string>(member.ImplementationMethodInfo.ReturnType, MutationScenario);
            loggingCallHandler.Builder = new MethodInvocationStringBuilder(argumentsMutator, returnMutator);

            return loggingCallHandler;
        }
    }
}
