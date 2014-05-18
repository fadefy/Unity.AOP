using System;

namespace Unity.AOP.Logging
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class ExcludeFromLogAttribute : Attribute
    {
    }
}
