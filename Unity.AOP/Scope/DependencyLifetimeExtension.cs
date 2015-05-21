using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Unity.AOP.Scope
{
    public class DependencyLifetimeExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<DependencyScopeStrategy>(UnityBuildStage.Lifetime);
        }
    }
}
