using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Utilities;

namespace Unity.AOP.Logging
{
    public class LoggingCallHandler : CallHandlerBase
    {
        private string _indent = null;
        private readonly IIndentSizeProvider _provider;

        public LoggingCallHandler(IIndentSizeProvider provider)
        {
            _provider = provider;
        }

        public IInovcationStringBuilder Builder { get; set; }

        public int IndentSize { get; set; }

        public bool IncludesArguments { get; set; }

        protected string IndentString
        {
            get { return _indent ?? (_indent = new string(' ', IndentSize)); }
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = null;
            Func<string, bool, string> logCall = (prefix, includeArguments) => Builder.Build(input, result, includeArguments);
            using (Hole.Of(_provider, i => i.Increase(), i => i.Decrease()))
            using (Hole.Of(logCall, log => log(IndentString + "Begin ", IncludesArguments), log => log(IndentString + "End ", false)))
                return result = getNext()(input, getNext);
        }
    }
}
