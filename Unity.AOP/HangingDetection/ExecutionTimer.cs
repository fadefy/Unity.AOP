using System;
using System.Diagnostics;

namespace Unity.AOP.HangingDetection
{
    public class ExecutionTimer : IEquatable<ExecutionTimer>
    {
        protected readonly long _startTicks;
        protected readonly static double _tickFrequency;

        static ExecutionTimer()
        {
            if (Stopwatch.IsHighResolution)
                _tickFrequency = 10000000.0 / Stopwatch.Frequency;
            else
                _tickFrequency = 1.0;
        }

        public ExecutionTimer()
        {
            _startTicks = Stopwatch.GetTimestamp();
        }

        public virtual TimeSpan Elapsed
        {
            get { return TimeSpan.FromTicks(GetElapsedDateTimeTicks()); }
        }

        public virtual long ElapsedTicks
        {
            get { return Stopwatch.GetTimestamp() - StartTicks; }
        }

        public virtual long StartTicks
        {
            get { return _startTicks; }
        }

        public virtual bool Equals(ExecutionTimer other)
        {
            return other == null ? false : other.StartTicks == StartTicks;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExecutionTimer);
        }

        public override int GetHashCode()
        {
            return StartTicks.GetHashCode();
        }

        protected long GetElapsedDateTimeTicks()
        {
            return (long)(ElapsedTicks * _tickFrequency);
        }
    }
}
