using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.AOP.Utilities;

namespace Unity.AOP.Caching
{
    public class CacheCallHandler : CallHandlerBase
    {
        protected readonly IArgumentsCacheKeyGenerator _keyGenerator;
        protected readonly IDictionary<object, IMethodReturn> _keyValueMap = new Dictionary<object, IMethodReturn>();

        public CacheCallHandler(IArgumentsCacheKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var inputArguments = input.Arguments.PeakArguments(info => !info.IsOut);
            var key = _keyGenerator.GenerateKey(input.MethodBase, inputArguments);
            IMethodReturn result = null;
            if (!_keyValueMap.TryGetValue(key, out result))
            {
                result = getNext()(input, getNext);
                _keyValueMap.Add(key, result);
            }

            return result;
        }
    }
}
