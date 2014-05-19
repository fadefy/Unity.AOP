using System;

namespace Unity.AOP.Caching
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CacheKeyAttribute : Attribute
    {
    }
}
