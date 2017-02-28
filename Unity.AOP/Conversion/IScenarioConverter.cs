using System;
using System.Collections.Generic;

namespace Unity.AOP.Mutation
{
    /// <summary>
    /// Defined a capability of mutating 
    /// </summary>
    public interface IScenarioConverter
    {
        IEnumerable<string> GetSupportedScenarios<T, TResult>();

        Func<T, TResult> Get<T, TResult>(string scenario = null);

        Func<object, object> Get(Type srouceType, Type targetType, string scenario = null);

        void Set<T, TResult>(Func<T, TResult> conversionMethod, string scenario = null);
    }
}
