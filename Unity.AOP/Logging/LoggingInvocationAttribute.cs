using System;

namespace Unity.AOP.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class LoggingInvocationAttribute : GenericHandlerAttribute
    {
        public LoggingInvocationAttribute()
            : base(typeof(LoggingCallHandler))
        {
            IndentSize = 2;
        }

        public int IndentSize { get; set; }

        public bool IncludesArguments { get; set; }
    }
}
