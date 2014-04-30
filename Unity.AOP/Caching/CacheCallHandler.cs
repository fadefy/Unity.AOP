using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Caching
{
    public class CacheCallHandler : AttributeDrivenCallHandlerBase<CacheResultAttribute>
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return null;
        }
    }
}
