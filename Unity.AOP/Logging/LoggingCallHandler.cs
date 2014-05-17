using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Utilities;

namespace Unity.AOP.Logging
{
    public class LoggingCallHandler : AttributeDrivenCallHandlerBase<LoggingInvocationAttribute>
    {
        private string _indent = null;

        protected string IndentString
        {
            get { return _indent ?? (_indent = new string(' ', Attribute.IndentSize)); }
        }

        [Dependency]
        public IInovcationStringBuilder Builder { get; set; }

        [Dependency]
        public IIndentSizeProvider Indent { get; set; }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = null;
            Func<string, bool, string> logCall = (prefix, includeArguments) => Builder.Build(input, result, includeArguments);
            using (Hole.Of(Indent, i => i.Increase(), i => i.Decrease()))
                using (Hole.Of(logCall, log => log(IndentString + "Begin ", Attribute.IncludesArguments), log => log(IndentString + "End ", Attribute.IncludesArguments)))
                    return result = getNext()(input, getNext);
        }
    }
}
