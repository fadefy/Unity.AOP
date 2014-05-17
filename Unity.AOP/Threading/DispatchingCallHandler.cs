using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Threading
{
    public class DispatchingCallHandler : AttributeDrivenCallHandlerBase<ThreadDispatchingAttribute>
    {
        private IInvocationDispatcher _dispatcher;

        public DispatchingCallHandler(IInvocationDispatcher dispatcher)
        {
            Contract.Assert(dispatcher != null);
            _dispatcher = dispatcher;
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return _dispatcher.Dispatch(GetSyncReturn(input, getNext), GetAsyncReturn(input, getNext));
        }

        protected virtual Func<IMethodReturn> GetSyncReturn(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return () => getNext()(input, getNext);
        }

        protected virtual Func<IMethodReturn> GetAsyncReturn(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return () => input.CreateMethodReturn(null, input.Arguments.Cast<object>().ToArray());
        }
    }
}
