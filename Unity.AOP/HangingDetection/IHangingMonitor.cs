using System;

namespace Unity.AOP.HangingDetection
{
    public interface IHangingMonitor : IDisposable
    {
        event EventHandler<InvocationHangEventArgs> InvocationHanged;

        void BeginMonitor(ExecutionRecord record);

        void EndMonitor(ExecutionRecord record);
    }
}
