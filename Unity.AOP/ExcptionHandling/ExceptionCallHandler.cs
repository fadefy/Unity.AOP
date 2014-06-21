using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Utilities;

namespace Unity.AOP.ExcptionHandling
{
    public class ExceptionCallHandler : AttributeDrivenCallHandlerBase<HandleExceptionAttribute>
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var nextCallHandler = getNext();
            var methodReturn = nextCallHandler(input, getNext);
            if (methodReturn.Exception != null)
            {
                Logger.Error("{0} throw exception {1}", input.MethodBase, methodReturn.Exception);
                var methodInfo = input.MethodBase as MethodInfo;
                return input.CreateMethodReturn(GetFallBackValue(methodInfo.ReturnType), BuildFallbackArguments(input));
            }

            return methodReturn;
        }

        protected virtual object[] BuildFallbackArguments(IMethodInvocation input)
        {
            var values = new List<object>();
            for (int i = 0; i < input.Arguments.Count;i++)
            {
                var value = input.Arguments[i];
                var info = input.Arguments.GetParameterInfo(i);
                if (info.IsOut && value == null)
                {
                    value = GetFallBackValue(info.ParameterType.GetElementType());
                }
                values.Add(value);
            }
            return values.ToArray();
        }

        protected virtual object GetFallBackValue(Type valueType)
        {
            if (valueType == null)
                return null;
            if (Attribute.FallbackValue != null)
                return Attribute.FallbackValue;
            if (Attribute.FallbackValueKey != null)
                return Container.Resolve(valueType, Attribute.FallbackValueKey);
            if (valueType.IsArray)
                return Array.CreateInstance(valueType.GetElementType(), 0);
            if (valueType.IsGenericType)
            {
                var genericArguments = valueType.GetGenericArguments();
                var fallBakType = (from cadidateType in new Type[] { typeof(List<>), typeof(Dictionary<,>) }
                                   where cadidateType.GetGenericArguments().Length == genericArguments.Length
                                   select cadidateType.MakeGenericType(genericArguments))
                                   .FirstOrDefault(type => valueType.IsAssignableFrom(type));
                if (fallBakType != null)
                {
                    return Activator.CreateInstance(fallBakType);
                }
            }

            return null;
        }
    }
}
