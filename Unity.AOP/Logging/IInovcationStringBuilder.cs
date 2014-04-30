using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Generic;

namespace Unity.AOP.Logging
{
    public interface IInovcationStringBuilder
    {
        string Build(IMethodInvocation invocation, IMethodReturn result, bool includesArguments);
    }
}
