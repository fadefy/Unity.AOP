using System;

namespace Unity.AOP.Logging
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LoggingTextAttribute : Attribute
    {
    }
}
