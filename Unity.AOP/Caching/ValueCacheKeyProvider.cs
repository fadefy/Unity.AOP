using System.Diagnostics.Contracts;
using System.Reflection;

namespace Unity.AOP.Caching
{
    public class ValueCacheKeyProvider : ICacheKeyProvider
    {
        [Pure]
        public object GenerateKey(MethodBase method, object[] arguments)
        {
            return string.Join(string.Empty, arguments);
        }
    }
}
