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
        private static string _scenario = "Logging";
        private int _indentSize = 4;

        public int IndentSize
        {
            get { return _indentSize; }
            set { _indentSize = value; }
        }

        public bool IncludesArguments { get; set; }

        public static string MutationScenario
        {
            get { return _scenario; }
            set { _scenario = value; }
        }

        public override ICallHandler CreateHandler(IUnityContainer container, MethodImplementationInfo member)
        {
            return container.Resolve<LoggingCallHandler>(
                new LambdaPropertyOverride<LoggingCallHandler>(h => h.Builder, CreateStringBuilder(member.ImplementationMethodInfo, container)),
                new LambdaPropertyOverride<LoggingCallHandler>(h => h.IncludesArguments, IncludesArguments),
                new LambdaPropertyOverride<LoggingCallHandler>(h => h.IndentSize, IndentSize),
                new LambdaPropertyOverride<LoggingCallHandler>(h => h.Order, Order));
        }

        protected virtual IInovcationStringBuilder CreateStringBuilder(MethodInfo methodInfo, IUnityContainer container)
        {
            var mutator = GetConverter(container);
            var arguments = methodInfo.GetParameters();
            var ignoredArgumentIndexes = (from argumentWithIndex in arguments.Select((p, i) => new { Parameter = p, Index = i })
                                          where argumentWithIndex.Parameter.GetCustomAttributes<ExcludeFromLogAttribute>().Any()
                                          select argumentWithIndex.Index).ToList();
            var argumentTypes = arguments.ExceptIndices(ignoredArgumentIndexes).Select(p => p.ParameterType).ToArray();
            var argumentsMutator = mutator.Generate<string>(argumentTypes, MutationScenario);
            var returnMutator = mutator.Generate<string>(methodInfo.ReturnType, MutationScenario);

            return new MethodInvocationStringBuilder(ignoredArgumentIndexes, argumentsMutator, returnMutator);
        }

        protected virtual IScenarioConverter GetConverter(IUnityContainer container)
        {
            return container.Resolve<IScenarioConverter>();
        }
    }
}
