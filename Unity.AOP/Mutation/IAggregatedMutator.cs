using System;
using System.Collections.Generic;

namespace Unity.AOP.Mutation
{
    /// <summary>
    /// Defined a capability of mutating 
    /// </summary>
    public interface IAggregatedMutator
    {
        IEnumerable<string> GetSupportedScenarios<T, TResult>();

        Mutate<T, TResult> GetMutator<T, TResult>(string scenario = null);

        Delegate GetMutator(Type srouceType, Type targetType, string scenario = null);

        void SetMutator<T, TResult>(Mutate<T, TResult> mutator, string scenario = null);
    }
}
