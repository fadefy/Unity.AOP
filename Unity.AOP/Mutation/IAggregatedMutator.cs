﻿using System;
using System.Collections.Generic;

namespace Unity.AOP.Mutation
{
    /// <summary>
    /// Defined a capability of mutating 
    /// </summary>
    public interface IAggregatedMutator
    {
        IEnumerable<string> GetSupportedScenarios<T, TResult>();

        Func<T, TResult> GetMutator<T, TResult>(string scenario = null);

        Func<object, object> GetMutator(Type srouceType, Type targetType, string scenario = null);

        void SetMutator<T, TResult>(Func<T, TResult> mutator, string scenario = null);
    }
}
