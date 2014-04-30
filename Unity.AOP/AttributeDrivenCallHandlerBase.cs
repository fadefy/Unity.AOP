using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.AOP
{
    public abstract class AttributeDrivenCallHandlerBase<T> : CallHandlerBase
        where T : HandlerAttribute
    {
        [Dependency]
        public virtual T Attribute { get; set; }
    }
}
