using System.Reflection;

namespace Unity.AOP.Caching
{
    public interface IArgumentsCacheKeyGenerator
    {
        object GenerateKey(MethodBase method, object[] arguments);
    }
}
