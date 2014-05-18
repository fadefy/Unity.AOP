using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Unity.AOP.Mutation
{
    public class AggregatedMutator : IAggregatedMutator
    {
        protected readonly ConcurrentDictionary<MutatorKey, Delegate> _mutators = new ConcurrentDictionary<MutatorKey, Delegate>();

        public AggregatedMutator()
        {
            SetMutator<char, string>(Convert.ToString);
            SetMutator<byte, string>(Convert.ToString);
            SetMutator<sbyte, string>(Convert.ToString);
            SetMutator<short, string>(Convert.ToString);
            SetMutator<int, string>(Convert.ToString);
            SetMutator<uint, string>(Convert.ToString);
            SetMutator<long, string>(Convert.ToString);
            SetMutator<ulong, string>(Convert.ToString);
            SetMutator<float, string>(Convert.ToString);
            SetMutator<double, string>(Convert.ToString);
            SetMutator<decimal, string>(Convert.ToString);
            SetMutator<DateTime, string>(d => d.ToString("O"));
        }

        public virtual IEnumerable<string> GetSupportedScenarios<T, TResult>()
        {
            return _mutators.Keys.Where(k => k.SourceType == typeof(T) && k.TargetType == typeof(TResult)).Select(k => k.Scenario);
        }

        public virtual Mutate<T, TResult> GetMutator<T, TResult>(string scenario = null)
        {
            return GetMutator(typeof(T), typeof(TResult), scenario) as Mutate<T, TResult>;
        }

        public virtual Delegate GetMutator(Type sourceType, Type targetType, string scenario = null)
        {
            Delegate mutator;
            var key = new MutatorKey(scenario, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            // Fail back to default if not find.
            key = new MutatorKey(null, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            throw new InvalidOperationException(string.Format("The mutation function from {0} to {1} has not been set yet.", sourceType, targetType));
        }

        public virtual void SetMutator<T, TResult>(Mutate<T, TResult> mutator, string scenario = null)
        {
            var key = new MutatorKey(scenario, typeof(T), typeof(TResult));

            _mutators.AddOrUpdate(key, mutator, (k, oldValue) => mutator);
        }
    }
}
