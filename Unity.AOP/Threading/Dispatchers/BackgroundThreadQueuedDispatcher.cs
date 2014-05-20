using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Unity.AOP.Threading.Dispatchers
{
    public class BackgroundThreadQueuedDispatcher : IInvocationDispatcher, IDisposable
    {
        protected readonly Task _pendingInvocationExecutor;
        protected readonly CancellationTokenSource _executorCancellationSource = new CancellationTokenSource();
        protected readonly ConcurrentQueue<Func<object>> _pendingInvocations = new ConcurrentQueue<Func<object>>();

        public BackgroundThreadQueuedDispatcher()
        {
            _pendingInvocationExecutor = Task.Factory.StartNew(ExecutePendingInvocations,
                _executorCancellationSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public DispatcherPriority Priority { get; set; }

        public TimeSpan Timeout { get; set; }

        public T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult)
        {
            _pendingInvocations.Enqueue(() => invocation());

            return createDefaultResult();
        }

        public void Dispose()
        {
            _executorCancellationSource.Cancel();
            _pendingInvocationExecutor.Dispose();
        }

        protected virtual void ExecutePendingInvocations()
        {
            var _pendingObservable = _pendingInvocations.ToObservable();
            while (!_executorCancellationSource.IsCancellationRequested)
            {
                while(!_pendingInvocations.IsEmpty)
                {
                    Func<object> action;
                    if (_pendingInvocations.TryPeek(out action))
                    {
                        ExecuteInvocationSafely(action);
                    }
                }
                // TODO: is it better to wait on an AutoResetEvent?
                Thread.Yield();
            }
        }

        protected void ExecuteInvocationSafely(Func<object> invocation)
        {
            try
            {
                invocation();
            }
            catch
            {
                // TODO: LOG
            }
        }
    }
}
