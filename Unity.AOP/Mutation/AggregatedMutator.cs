using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Unity.AOP.Mutation
{
    public class AggregatedMutator : IAggregatedMutator
    {
        protected readonly ConcurrentDictionary<MutatorKey, Func<object, object>> _mutators = new ConcurrentDictionary<MutatorKey, Func<object, object>>();

        public virtual IEnumerable<string> GetSupportedScenarios<T, TResult>()
        {
            return _mutators.Keys.Where(k => k.SourceType == typeof(T) && k.TargetType == typeof(TResult)).Select(k => k.Scenario);
        }

        public virtual Func<T, TResult> GetMutator<T, TResult>(string scenario = null)
        {
            var mutator = GetMutator(typeof(T), typeof(TResult), scenario);

            return value => (TResult)mutator(value);
        }

        public virtual Func<object, object> GetMutator(Type sourceType, Type targetType, string scenario = null)
        {
            if (sourceType.IsByRef)
                sourceType = sourceType.GetElementType();
            if (targetType.IsByRef)
                targetType = sourceType.GetElementType();
            if (sourceType == targetType)
                return new Func<object, object>(JustReturn);
            if (sourceType == typeof(void))
                return new Func<object, object>(JustReturn);

            Func<object, object> mutator;
            var key = new MutatorKey(scenario, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            // Fail back to default if not find.
            key = new MutatorKey(null, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            throw new InvalidOperationException(string.Format("The mutation function from {0} to {1} has not been set yet.", sourceType, targetType));
        }

        public virtual void SetMutator<T, TResult>(Func<T, TResult> mutator, string scenario = null)
        {
            var key = new MutatorKey(scenario, typeof(T), typeof(TResult));
            Func<object, object> objMutator = obj => mutator((T)obj);

            _mutators.AddOrUpdate(key, k => objMutator, (k, oldValue) => objMutator);
        }

        protected T JustReturn<T>(T v)
        {
            return v;
        }
    }
}
