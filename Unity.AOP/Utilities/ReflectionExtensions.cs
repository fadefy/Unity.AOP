using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Utilities
{
    public static class ReflectionExtensions
    {
        public static object[] PeakArguments(this IParameterCollection parameters, Predicate<ParameterInfo> filter)
        {
            var inputArguments = new List<object>();
            for (int i = 0; i < parameters.Count; i++)
            {
                var info = parameters.GetParameterInfo(i);
                if (filter(info))
                {
                    inputArguments.Add(parameters[i]);
                }
            }

            return inputArguments.ToArray();
        }
    }
}
