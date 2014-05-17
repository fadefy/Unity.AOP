using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.AOP.ExcptionHandling;
using Unity.AOP.Test.Framework;

namespace Unity.AOP.Test.CallHandlers
{
    [TestClass]
    public class ExceptionCallHandlerTest : TestBase
    {
        public class ThrowException
        {
            [HandleException]
            public virtual void SwollowException()
            {
                throw new InvalidOperationException();
            }

            [HandleException]
            public virtual IEnumerable<string> ReturnGenericContainer()
            {
                throw new InvalidOperationException();
            }

            [HandleException]
            public virtual IEnumerable<string> ReturnArray()
            {
                throw new InvalidOperationException();
            }

            [HandleException]
            public virtual IDictionary<string, string> ReturnDictionary()
            {
                throw new InvalidOperationException();
            }

            [HandleException(FallbackValue = null)]
            public virtual IEnumerable<string> ReturnFallbackValue()
            {
                throw new InvalidOperationException();
            }

            [HandleException]
            public virtual void FillInContainer(out IEnumerable<string> names)
            {
                throw new InvalidOperationException();
            }
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<ThrowException>(new PerResolveLifetimeManager(),
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
        }

        [TestMethod]
        public void ExceptionHandingTest()
        {
            var api = Container.Resolve<ThrowException>();
            api.SwollowException();
        }

        [TestMethod]
        public void OutValueFallbackToEmptyTest()
        {
            var api = Container.Resolve<ThrowException>();
            IEnumerable<string> result;
            api.FillInContainer(out result);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void FallbackToAttributeTest()
        {
            var api = Container.Resolve<ThrowException>();
            Assert.IsNull(api.ReturnFallbackValue());
        }

        [TestMethod]
        public void FallbackWithEmptyTest()
        {
            var api = Container.Resolve<ThrowException>();
            Assert.IsNotNull(api.ReturnGenericContainer());
            Assert.IsNotNull(api.ReturnDictionary());
            Assert.IsNotNull(api.ReturnArray());
        }
    }
}
