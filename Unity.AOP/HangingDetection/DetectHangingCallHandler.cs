using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Diagnostics.Contracts;
using Unity.AOP.Utilities;

namespace Unity.AOP.HangingDetection
{
    public class DetectHangingCallHandler : AttributeDrivenCallHandlerBase<DetectHangingAttribute>
    {
        private IHangingMonitor _monitor;

        [Dependency]
        public IHangingMonitor HangingMonitor
        {
            get { return _monitor; }
            set
            {
                Contract.Assert(value != null);
                if (_monitor != null)
                    _monitor.InvocationHanged -= OnInvocationHanged;
                _monitor = value;
                _monitor.InvocationHanged += OnInvocationHanged;
            }
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            using (Hole.Of(CreateRecord(input), HangingMonitor.BeginMonitor, HangingMonitor.EndMonitor))
                return getNext()(input, getNext);
        }

        protected virtual ExecutionRecord CreateRecord(IMethodInvocation invocation)
        {
            return new ExecutionRecord(invocation.MethodBase, Attribute);
        }

        protected virtual void OnInvocationHanged(object sender, InvocationHangEventArgs e)
        {
            RaiseCompositeEvent(e.ExecutionRecrod);
        }
    }
}
