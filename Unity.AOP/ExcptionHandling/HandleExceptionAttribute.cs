using System;

namespace Unity.AOP.ExcptionHandling
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class HandleExceptionAttribute : GenericHandlerAttribute
    {
        public HandleExceptionAttribute()
            : base(typeof(ExceptionCallHandler))
        {
        }

        public object FallbackValue { get; set; }

        public string FallbackValueKey { get; set; }
    }
}
