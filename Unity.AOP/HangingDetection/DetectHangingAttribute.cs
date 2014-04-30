using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace Unity.AOP.HangingDetection
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DetectHangingAttribute : GenericHandlerAttribute 
    {
        public DetectHangingAttribute()
            : base(typeof(DetectHangingCallHandler))
        {
        }

        /// <summary>
        /// Unit in MS.
        /// </summary>
        public double MaxExpectedExecutionTime { get; set; }

        public bool TerminateExecutingThreadOnHanding { get; set; }
    }
}
