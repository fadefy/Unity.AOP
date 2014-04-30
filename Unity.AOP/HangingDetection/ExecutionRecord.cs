using System;
using System.Reflection;
using System.Threading;

namespace Unity.AOP.HangingDetection
{
    public class ExecutionRecord : IEquatable<ExecutionRecord>
    {
        private readonly ExecutionTimer _timer;
        private readonly MethodBase _executionMethod;
        private readonly DetectHangingAttribute _detectionAttribute;
        private readonly Thread _executionThread;

        public ExecutionRecord(MethodBase executingMethod, DetectHangingAttribute attribute = null)
        {
            _executionMethod = executingMethod;
            _detectionAttribute = attribute ?? executingMethod.GetCustomAttribute<DetectHangingAttribute>(true);
            _executionThread = Thread.CurrentThread;
            _timer = new ExecutionTimer();
        }

        public static implicit operator ExecutionRecord(MethodBase methodInfo)
        {
            return new ExecutionRecord(methodInfo);
        }

        public MethodBase ExecutionMethod
        {
            get { return _executionMethod; }
        }

        public Thread ExecutionThread
        {
            get { return _executionThread; }
        }

        public virtual TimeSpan MaxExpectedExecutionTime
        {
            get { return TimeSpan.FromMilliseconds(_detectionAttribute.MaxExpectedExecutionTime); }
        }

        public virtual bool IsHanding
        {
            get
            {
                return ExecutedTime > MaxExpectedExecutionTime ?
                ExecutionThread.ThreadState == ThreadState.WaitSleepJoin : false;
            }
        }

        public virtual TimeSpan ExecutedTime
        {
            get { return _timer.Elapsed; }
        }

        public virtual bool Equals(ExecutionRecord other)
        {
            return other == null ? false :
                   ExecutionThread == other.ExecutionThread && _timer == other._timer;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExecutionRecord);
        }

        public override int GetHashCode()
        {
            return ExecutionThread.GetHashCode() ^ _timer.GetHashCode();
        }
    }
}
