using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Linq;
using Unity.AOP.Utilities;

namespace Unity.AOP.Scope
{
    public class DependencyScopeStrategy : BuilderStrategy
    {
        private DualKeyDictionary<Type, ArgumentsList, WeakReference> _cache = new DualKeyDictionary<Type, ArgumentsList, WeakReference>();

        public override void PreBuildUp(IBuilderContext context)
        {
            if (context.BuildComplete)
                return;
            if (context.Existing != null)
                return;
            var arguments = context.ResolveConstructorScopedArguments().ToArray();
            if (arguments.Length == 0)
                return;

            var existing = _cache.Get(context.BuildKey.Type, new ArgumentsList(arguments));
            if (existing == null || !existing.IsAlive)
                return;

            context.Existing = existing.Target;
            context.BuildComplete = true;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            var arguments = context.ResolveConstructorScopedArguments().ToArray();
            if (arguments.Length > 0)
            {
                _cache.TryAdd(context.BuildKey.Type, new ArgumentsList(arguments), new WeakReference(context.Existing));
            }
        }
    }
}
