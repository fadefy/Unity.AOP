using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Logging
{
    public class LoggingCallHandler : AttributeDrivenCallHandlerBase<LoggingInvocationAttribute>
    {
        [Dependency]
        public IInovcationStringBuilder Builder { get; set; }

        [Dependency]
        public IIndentSizeProvider Indent { get; set; }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var nextCallHandler = getNext();
            var methodReturn = nextCallHandler(input, getNext);
            Info("Calling {0}", input.MethodBase);

            return methodReturn;
        }
    }
}
