using System;
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
        protected readonly Func<IEnumerable<object>, IEnumerable<string>> _parameterMutator;
        protected readonly IList<int> _excludedIndices;

        public MethodInvocationStringBuilder(IList<int> excludedIndices, Func<IEnumerable<object>, IEnumerable<string>> parameterMutator, Func<object, string> returnMutator)
        {
            _returnMutator = returnMutator;
            _parameterMutator = parameterMutator;
            _excludedIndices = excludedIndices;
        }

        public virtual string Build(IMethodInvocation invocation, IMethodReturn result, bool includesArguments)
        {
            var needAppendReturn = result != null && result.ReturnValue != null;
            var parameters = includesArguments ? _parameterMutator(invocation.Arguments.Cast<object>().Exclude(_excludedIndices)) : new List<string>();
            return new StringBuilder()
                   .Append(invocation.MethodBase.Name)
                   .AppendFormat("({0})", string.Join(", ", parameters))
                   .AppendIf(needAppendReturn, () => " return " + _returnMutator(result.ReturnValue))
                   .ToString();
        }
    }
}
