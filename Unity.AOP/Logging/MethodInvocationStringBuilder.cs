﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Utilities;

namespace Unity.AOP.Logging
{
    /// <summary>
    /// Each instance only expected to work for a specific method for better performance.
    /// </summary>
    public class MethodInvocationStringBuilder : IInovcationStringBuilder
    {
        protected readonly Func<object, string> _returnMutator;
        protected readonly Func<IEnumerable<object>, IList<string>> _parameterMutator;
        protected readonly IList<int> _excludedIndices;

        public MethodInvocationStringBuilder(IList<int> excludedIndices, Func<IEnumerable<object>, IList<string>> parameterMutator, Func<object, string> returnMutator)
        {
            _returnMutator = returnMutator;
            _parameterMutator = parameterMutator;
            _excludedIndices = excludedIndices;
        }

        public virtual string Build(IMethodInvocation invocation, IMethodReturn result, bool includesArguments)
        {
            var parameters = includesArguments ? _parameterMutator(invocation.Arguments.Cast<object>().Exclude(_excludedIndices)) : new List<string>();
            var stringBuilder = new StringBuilder()
                .Append(invocation.MethodBase.Name)
                .AppendFormat("({0})", string.Join(", ", parameters));
            if (result != null && result.ReturnValue != null)
                stringBuilder.Append(" return " + _returnMutator(result.ReturnValue));

            return stringBuilder.ToString();
        }
    }
}
