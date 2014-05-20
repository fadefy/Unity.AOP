using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Unity.AOP.Caching
{
    public class ArgumentsStringCacheKeyGenerator : IArgumentsCacheKeyGenerator
    {
        private readonly Func<IEnumerable<object>, IEnumerable<string>> _argumentMutator;

        public ArgumentsStringCacheKeyGenerator(Func<IEnumerable<object>, IEnumerable<string>> argumentsMutator)
        {
            _argumentMutator = argumentsMutator;
        }

        [Pure]
        public object GenerateKey(MethodBase method, IEnumerable<object> arguments)
        {
            return string.Join(string.Empty, _argumentMutator(arguments));
        }
    }
}
