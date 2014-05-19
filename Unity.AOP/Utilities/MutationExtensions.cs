using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AOP.Mutation;

namespace Unity.AOP.Utilities
{
    public static class MutationExtensions
    {
        public static IAggregatedMutator RegistBasicMutators(this IAggregatedMutator mutators)
        {
            mutators.SetMutator<char, string>(Convert.ToString);
            mutators.SetMutator<byte, string>(Convert.ToString);
            mutators.SetMutator<sbyte, string>(Convert.ToString);
            mutators.SetMutator<short, string>(Convert.ToString);
            mutators.SetMutator<int, string>(Convert.ToString);
            mutators.SetMutator<uint, string>(Convert.ToString);
            mutators.SetMutator<long, string>(Convert.ToString);
            mutators.SetMutator<ulong, string>(Convert.ToString);
            mutators.SetMutator<float, string>(Convert.ToString);
            mutators.SetMutator<double, string>(Convert.ToString);
            mutators.SetMutator<decimal, string>(Convert.ToString);
            mutators.SetMutator<DateTime, string>(d => d.ToString("O"));
            return mutators;
        }

        public static Func<IEnumerable<object>, IList<T>> GenerateMutator<T>(this IAggregatedMutator aggregatedMutator, Type[] parameterTypes, string scenario = null)
        {
            var mutators = parameterTypes.Select(type => aggregatedMutator.GenerateMutator<T>(type, scenario)).ToList();

            return values => values.Zip(mutators, (value, mutator) => mutator(value)).ToList();
        }

        public static Func<object, T> GenerateMutator<T>(this IAggregatedMutator aggregatedMutator, Type sourceType, string scenario = null)
        {
            var mutator = aggregatedMutator.GetMutator(sourceType, typeof(T), scenario);

            return value => (T)mutator(value);
        }
    }
}
