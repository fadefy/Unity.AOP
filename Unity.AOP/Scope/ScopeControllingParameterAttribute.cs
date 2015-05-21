using System;

namespace Unity.AOP.Scope
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ScopeControllingParameterAttribute : Attribute
    {
    }
}
