using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Framework
{
    public abstract class PerMethodHandlerAttribute : Attribute
    {
        public int Order { get; set; }

        public abstract ICallHandler CreateHandler(IUnityContainer container, MethodImplementationInfo member);
    }
}
