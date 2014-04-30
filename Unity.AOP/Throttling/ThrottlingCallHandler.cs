using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace Unity.AOP.Throttling
{
    public class ThrottlingCallHandler : AttributeDrivenCallHandlerBase<ThrottlingInvocationAttribute>
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            throw new NotImplementedException();
        }
    }
}
