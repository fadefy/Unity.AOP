using System;

namespace Unity.AOP.HangingDetection
{
    public class InvocationHangEventArgs : EventArgs
    {
        public ExecutionRecord ExecutionRecrod { get; set; }
    }
}
