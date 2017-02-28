using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Unity.AOP.Mutation
{
    public class ScenarioConverter : IScenarioConverter
    {
        protected readonly ConcurrentDictionary<ConvertionKey, Func<object, object>> _mutators = new ConcurrentDictionary<ConvertionKey, Func<object, object>>();

        public virtual IEnumerable<string> GetSupportedScenarios<T, TResult>()
        {
            return _mutators.Keys.Where(k => k.SourceType == typeof(T) && k.TargetType == typeof(TResult)).Select(k => k.Scenario);
        }

        public virtual Func<T, TResult> Get<T, TResult>(string scenario = null)
        {
            var mutator = Get(typeof(T), typeof(TResult), scenario);

            return value => (TResult)mutator(value);
        }

        public virtual Func<object, object> Get(Type sourceType, Type targetType, string scenario = null)
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
            var key = new ConvertionKey(scenario, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            // Fail back to default if not find.
            key = new ConvertionKey(null, sourceType, targetType);
            if (_mutators.TryGetValue(key, out mutator))
                return mutator;

            throw new InvalidOperationException(string.Format("The mutation function from {0} to {1} has not been set yet.", sourceType, targetType));
        }

        public virtual void Set<T, TResult>(Func<T, TResult> conversionMethod, string scenario = null)
        {
            var key = new ConvertionKey(scenario, typeof(T), typeof(TResult));
            Func<object, object> objMutator = obj => conversionMethod((T)obj);

            _mutators.AddOrUpdate(key, (ConvertionKey k) => objMutator, (k, oldValue) => objMutator);
        }

        protected T JustReturn<T>(T v)
        {
            return v;
        }
    }
}
