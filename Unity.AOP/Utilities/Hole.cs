using System;
using System.Diagnostics.Contracts;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.ServiceLocation;

namespace Unity.AOP.Utilities
{
    public class Hole
    {
        public static IDisposable Of<T>(T target, Action<T> entryAction, Action<T> exitAction)
        {
            return new DisposibleHole<T>(target, entryAction, exitAction);
        }

        /// <summary>
        /// Apply entryAction on targetFactory result if target created successfully.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetFactory"></param>
        /// <param name="entryAction"></param>
        /// <param name="exitAction"></param>
        /// <returns>An disposible object runs exitAction when disposing if entryAction was successfully executed.</returns>
        /// <remarks>All exceptions thrown within any of input delegates will be cought and log into <c>Microsoft.Practices.Prism.Logging.ILoggerFacade</c>.</remarks>
        public static IDisposable OfTry<T>(Func<T> targetFactory, Action<T> entryAction, Action<T> exitAction)
        {
            return new TryHole<T>(targetFactory, entryAction, exitAction);
        }

        public static IDisposable OfTryFinal<T>(Func<T> targetFactory, Action<T> entryAction, Action<T> exitAction)
        {
            return new FinalHole<T>(targetFactory, entryAction, exitAction);
        }

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

        private class TryHole<T> : IDisposable
        {
            private Action _exitAction = () => { };

            public TryHole(Func<T> targetFactory, Action<T> entryAction, Action<T> exitAction)
            {
                Contract.Assert(entryAction != null);
                Contract.Assert(exitAction != null);
                TryCall(() =>
                {
                    var target = targetFactory();
                    TryCall(() =>
                    {
                        entryAction(target);
                        _exitAction = () =>
                        TryCall(() => exitAction(target));
                    });
                });
            }

            public void Dispose()
            {
                _exitAction();
            }
        }

        private class FinalHole<T> : IDisposable
        {
            private Action _exitAction = () => { };

            public FinalHole(Func<T> targetFactory, Action<T> entryAction, Action<T> exitAction)
            {
                Contract.Assert(entryAction != null);
                Contract.Assert(exitAction != null);
                TryCall(() =>
                {
                    var target = targetFactory();
                    TryCall(() => entryAction(target));
                    _exitAction = () =>
                    TryCall(() => exitAction(target));
                });
            }

            public void Dispose()
            {
                _exitAction();
            }
        }

        private static void TryCall(Action action, Action failAction = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var loggerFacade = ServiceLocator.Current.GetInstance<ILoggerFacade>();
                loggerFacade.Error(ex.ToString());
                if (failAction != null)
                    failAction();
            }
        }
    }
}
