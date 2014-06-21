using System;

namespace Unity.AOP.Utilities
{
    public static class DelegationExtentions
    {
        public static Action FailWith(this Action action, Action<Exception> failAction)
        {
            return () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    failAction(ex);
                }
            };
        }

        public static Action Then(this Action action, Action next)
        {
            return () =>
            {
                action();
                next();
            };
        }

        public static Func<T> Then<T>(this Func<T> func, Action next)
        {
            return () =>
            {
                var result = func();
                next();
                return result;
            };
        }
    }
}
