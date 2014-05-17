﻿using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP
{
    public abstract class GenericHandlerAttribute : HandlerAttribute
    {
        public Type HandlerType { get; set; }

        public GenericHandlerAttribute(Type handlerType)
        {
            HandlerType = handlerType;
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return (ICallHandler)container.Resolve(HandlerType, GetHandlerConstructionOverride());
        }

        protected virtual ResolverOverride[] GetHandlerConstructionOverride()
        {
            return new []
            {
                new PropertyOverride("Order", Order),
                new PropertyOverride("Attribute", this)
            };
        }
    }
}
