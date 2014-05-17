﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Unity.AOP.Threading.Dispatchers
{
    public class BackgroundThreadDispatcher : IInvocationDispatcher
    {
        public TimeSpan Timeout { get;set; } = TimeSpan.MaxValue;

        public DispatcherPriority Priority { get; set; }

        public virtual T Dispatch<T>(Func<T> invocation, Func<T> createDefaultResult)
        {
            Contract.Requires<ArgumentNullException>(invocation != null, "invocation cannot be null.");
            var task = Task.Factory.StartNew(invocation, TaskCreationOptions.PreferFairness);
            task.ContinueWith(HandleTaskResult);
            task.Wait(Timeout);

            if (task.IsCompleted)
                return task.Result;
            else
                throw task.Exception;
        }

        protected virtual void HandleTaskResult<T>(Task<T> previousTask)
        {
        }
    }
}
