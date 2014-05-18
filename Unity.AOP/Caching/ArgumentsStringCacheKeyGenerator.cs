using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Unity.AOP.Caching
{
    public class ArgumentsStringCacheKeyGenerator : IArgumentsCacheKeyGenerator
    {
        private readonly Func<IEnumerable<object>, IList<string>> _argumentMutator;

        public ArgumentsStringCacheKeyGenerator(Func<IEnumerable<object>, IList<string>> argumentsMutator)
        {
            _argumentMutator = argumentsMutator;
        }

        [Pure]
        public object GenerateKey(MethodBase method, object[] arguments)
        {
            return string.Join(string.Empty, _argumentMutator(arguments));
        }
    }
}
