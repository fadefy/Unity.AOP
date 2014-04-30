using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Unity.AOP.Utilities;

namespace Unity.AOP.HangingDetection
{
    using ThreadRecrods = ConcurrentStack<ExecutionRecord>;

    public class ConcurrentHangingMonitor : IHangingMonitor
    {
        protected readonly ConcurrentDictionary<Thread, ThreadRecrods> _monitoredRecords = new ConcurrentDictionary<Thread, ThreadRecrods>();
        protected readonly IDisposable _monitor;

        public ConcurrentHangingMonitor()
        {
            InvocationHanged += (sender, e) => { };
            _monitor = new Action(DetectHangingRecords).ToAsync()
                      .RepeatWithInterval(TimeSpan.FromMilliseconds(200))
                      .Subscribe();
        }

        public virtual event EventHandler<InvocationHangEventArgs> InvocationHanged;

        public virtual void BeginMonitor(ExecutionRecord record)
        {
            Contract.Assert(record != null);
            GetCurrentRecords().Push(record);
        }

        public virtual void EndMonitor(ExecutionRecord end)
        {
            Contract.Assert(end != null);
            ExecutionRecord record;
            if (GetCurrentRecords().TryPop(out record))
                Contract.Assert(record.Equals(end));
        }
        public virtual void Dispose()
        {
            _monitor.Dispose();
        }

        protected virtual ThreadRecrods GetCurrentRecords()
        {
            return _monitoredRecords.GetOrAdd(Thread.CurrentThread, _ => new ThreadRecrods());
        }

        protected virtual IEnumerable<ExecutionRecord> SnapshotRecords(Func<ExecutionRecord, bool> predicate)
        {
            return _monitoredRecords.Values.SelectMany(stack => stack.ToArray()).Where(predicate);
        }

        protected virtual void DetectHangingRecords()
        {
            SnapshotRecords(that => that.IsHanding).ToList()
                .ForEach(NotifyHangingRecord);
        }

        protected virtual void NotifyHangingRecord(ExecutionRecord record)
        {
            InvocationHanged(this, new InvocationHangEventArgs() { ExecutionRecrod = record });
        }
    }
}
