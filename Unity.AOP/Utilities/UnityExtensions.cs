using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.AOP.Scope;

namespace Unity.AOP.Utilities
{
    public static class UnityExtensions
    {
        public static SelectedConstructor SelectConstructor(this IBuilderContext context)
        {
            IPolicyList policyList;
            var constructorSelectorPolicy = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out policyList);
            return constructorSelectorPolicy.SelectConstructor(context, policyList);
        }

        public static IEnumerable<ParameterInfo> FindScopedParameters(this MethodBase constructor)
        {
            return constructor.GetParameters().Where(p => p.GetCustomAttributes<ScopeControllingParameterAttribute>().Any());
        }

        public static IEnumerable<object> ResolveConstructorScopedArguments(this IBuilderContext context)
        {
            var constructor = context.SelectConstructor();
            if (constructor == null)
                throw new InvalidOperationException("Call after TypeMapping Stage.");

            var operation = context.CurrentOperation;
            foreach(var p in constructor.Constructor.FindScopedParameters())
            {
                context.CurrentOperation = new MethodArgumentResolveOperation(p.ParameterType, null, p.Name);
                yield return context.GetOverriddenResolver(context.BuildKey.Type).Resolve(context);
            }
            context.CurrentOperation = operation;
        }
    }
}
