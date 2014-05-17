using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP.Caching
{
    public class CacheCallHandler : AttributeDrivenCallHandlerBase<CacheResultAttribute>
    {
        protected ICacheKeyProvider _keyProvider;
        protected IDictionary<object, IMethodReturn> _keyValueMap = new Dictionary<object, IMethodReturn>();

        public CacheCallHandler(ICacheKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var key = _keyProvider.GenerateKey(input.MethodBase, GetInputArguments(input));
            IMethodReturn result = null;
            if (!_keyValueMap.TryGetValue(key, out result))
            {
                result = getNext()(input, getNext);
                _keyValueMap.Add(key, result);
            }

            return result;
        }

        protected virtual object[] GetInputArguments(IMethodInvocation input)
        {
            var inputArguments = new List<object>();
            for (int i = 0; i < input.Arguments.Count; i++)
            {
                var info = input.Arguments.GetParameterInfo(i);
                if (!info.IsOut)
                {
                    inputArguments.Add(input.Arguments[i]);
                }
            }

            return inputArguments.ToArray();
        }
    }
}
