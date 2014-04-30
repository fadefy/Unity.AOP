using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Logging
{
    public class InvocationStringBuilder : IInovcationStringBuilder
    {
        public virtual string Build(IMethodInvocation invocation, IMethodReturn result, bool includesArguments)
        {
            return null;
        }
    }
}
