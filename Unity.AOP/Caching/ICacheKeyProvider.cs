
using System.Reflection;

namespace Unity.AOP.Caching
{
    public interface ICacheKeyProvider
    {
        object GenerateKey(MethodBase method, object[] arguments);
    }
}
