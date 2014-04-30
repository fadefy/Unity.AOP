using System;
using Unity.AOP.ExcptionHandling;
using Unity.AOP.HangingDetection;
using Unity.AOP.Logging;

namespace Unity.AOP.Demo.Services
{
    public class SoapSender : ISoapSender
    {
        [DetectHanging(MaxExpectedExecutionTime = 100)]
        [HandleException]
        [LoggingInvocation]
        public virtual void Send(SoapRequest request)
        {
            throw new NotSupportedException("Mock exception.");
        }
    }
}
