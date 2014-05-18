using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AOP.Mutation;

namespace Unity.AOP.Utilities
{
    public static class MutationExtensions
    {
        public static Func<IEnumerable<object>, IList<T>> GenerateMutator<T>(this IAggregatedMutator _mutators, Type[] parameterTypes, string scenario = null)
        {
            var mutators = parameterTypes.Select(type => _mutators.GenerateMutator<T>(type, scenario)).ToList();

            return values => values.Zip(mutators, (value, mutator) => (T)mutator.DynamicInvoke(value)).ToList();
        }

        public static Func<object, T> GenerateMutator<T>(this IAggregatedMutator _mutators, Type sourceType, string scenario = null)
        {
            var mutator = typeof(T) == sourceType ? new Func<T, T>(i => i) :
                          _mutators.GetMutator(sourceType, typeof(T), scenario);

            return value => (T)mutator.DynamicInvoke(value);
        }
    }
}
