using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Logging
{
    public interface IInovcationStringBuilder
    {
        string Build(IMethodInvocation invocation, IMethodReturn result, bool includesArguments);
    }
}
