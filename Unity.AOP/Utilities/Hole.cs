using System;
using System.Diagnostics.Contracts;

namespace Unity.AOP.Utilities
{
    public class Hole
    {
        private class DisposibleHole<T> : IDisposable
        {
            private readonly Action _exitAction;

            public DisposibleHole(T target, Action<T> entryAction, Action<T> exitAction)
            {
                Contract.Assert(entryAction != null);
                Contract.Assert(exitAction != null);

                _exitAction = () => exitAction(target);
                entryAction(target);
            }

            public void Dispose()
            {
                _exitAction();
            }
        }

        public static IDisposable Of<T>(T target, Action<T> entryAction, Action<T> exitAction)
        {
            return new DisposibleHole<T>(target, entryAction, exitAction);
        }

        public static IDisposable Of(Action entryAction, Action exitAction)
        {
            return new DisposibleHole<object>(null, _ => entryAction(), _ => exitAction());
        }
    }
}
