using Microsoft.Practices.Unity.InterceptionExtension;
using System.Linq;

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
                Error("{0} throw exception {1}", input.MethodBase, methodReturn.Exception);
                return input.CreateMethodReturn(Attribute.FallbackValue, input.Arguments.Cast<object>().ToArray());
            }

            return methodReturn;
        }
    }
}
