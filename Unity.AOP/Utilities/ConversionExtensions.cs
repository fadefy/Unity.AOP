using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AOP.Mutation;

namespace Unity.AOP.Utilities
{
    public static class ConversionExtensions
    {
        public static IScenarioConverter RegistBasicConverters(this IScenarioConverter converter)
        {
            converter.Set<char, string>(Convert.ToString);
            converter.Set<byte, string>(Convert.ToString);
            converter.Set<sbyte, string>(Convert.ToString);
            converter.Set<short, string>(Convert.ToString);
            converter.Set<int, string>(Convert.ToString);
            converter.Set<uint, string>(Convert.ToString);
            converter.Set<long, string>(Convert.ToString);
            converter.Set<ulong, string>(Convert.ToString);
            converter.Set<float, string>(Convert.ToString);
            converter.Set<double, string>(Convert.ToString);
            converter.Set<decimal, string>(Convert.ToString);
            converter.Set<DateTime, string>(d => d.ToString("O"));
            return converter;
        }

        public static Func<IEnumerable<object>, IEnumerable<T>> Generate<T>(this IScenarioConverter converter, Type[] parameterTypes, string scenario = null)
        {
            var converters = parameterTypes.Select(type => converter.Generate<T>(type, scenario)).ToList();

            return values => values.Zip(converters, (value, mutator) => mutator(value));
        }

        public static Func<object, T> Generate<T>(this IScenarioConverter converter, Type sourceType, string scenario = null)
        {
            var mutator = converter.Get(sourceType, typeof(T), scenario);

            return value => (T)mutator(value);
        }
    }
}
