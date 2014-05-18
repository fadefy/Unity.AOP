using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;
using ReflectionHelper = Microsoft.Practices.Unity.InterceptionExtension.ReflectionHelper;

namespace Unity.AOP.Framework
{
    /// <summary>
    /// This policy is create simply for a sake of built-in AttibuteDrivenPolicy. 
    /// It doesn't pass the MethodImplmentionInfo as an argument when creating call handlers. 
    /// While, that information could be useful while creating some call handler.
    /// </summary>
    public class PerMethodAttributeDrivenPolicy : InjectionPolicy
    {
        private readonly AttributeDrivenPolicyMatchingRule _attributeMatchRule;

        public PerMethodAttributeDrivenPolicy() : base("Per Method Attribute Driven Policy")
        {
            _attributeMatchRule = new AttributeDrivenPolicyMatchingRule();
        }

        protected override bool DoesMatch(MethodImplementationInfo member)
        {
            Guard.ArgumentNotNull(member, "member");
            return (member.InterfaceMethodInfo != null && _attributeMatchRule.Matches(member.InterfaceMethodInfo)) ||
                   _attributeMatchRule.Matches(member.ImplementationMethodInfo);
        }

        protected override IEnumerable<ICallHandler> DoGetHandlersFor(MethodImplementationInfo member, IUnityContainer container)
        {
            if (member.InterfaceMethodInfo != null)
            {
                foreach(var handlerAttribute in ReflectionHelper.GetAllAttributes<PerMethodHandlerAttribute>(member.InterfaceMethodInfo, true))
                {
                    yield return handlerAttribute.CreateHandler(container, member);
                }
            }
            foreach(var handlerAttribute in ReflectionHelper.GetAllAttributes<PerMethodHandlerAttribute>(member.ImplementationMethodInfo, true))
            {
                yield return handlerAttribute.CreateHandler(container, member);
            }
        }
    }
}
