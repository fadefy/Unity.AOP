using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.AOP.Caching;
using Unity.AOP.Test.Framework;

namespace Unity.AOP.Test.Unity
{
    [TestClass]
    public class CacheCallHandlerTest : TestBase
    {
        public class NeedToCache
        {
            public int InvocationCount { get; set; }

            [CacheResult]
            public virtual string SayHellow(string name)
            {
                InvocationCount++;
                return "Hello" + name;
            }

            [CacheResult]
            public virtual void SayHellow(string name, out string greeting)
            {
                InvocationCount++;
                greeting = "Hello" + name;
            }

            [CacheResult]
            public virtual string SayHellow(out string greeting, string name)
            {
                InvocationCount++;
                return greeting = "Hello" + name;
            }
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<NeedToCache>(new PerResolveLifetimeManager(),
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
        }

        [TestMethod]
        public void ReturnValueCacheTest()
        {
            var api = Container.Resolve<NeedToCache>();
            api.SayHellow("Hugo");
            var greeting = api.SayHellow("Hugo");

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            greeting = api.SayHellow("Hugo's Wife");
            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(2, api.InvocationCount);
        }

        [TestMethod]
        public void OutValueCacheTest()
        {
            var api = Container.Resolve<NeedToCache>();
            string greeting;
            api.SayHellow("Hugo", out greeting);
            api.SayHellow("Hugo", out greeting);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            api.SayHellow("Hugo's Wife", out greeting);
            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(2, api.InvocationCount);
        }

        [TestMethod]
        public void OutAndReturnValueCacheTest()
        {
            var api = Container.Resolve<NeedToCache>();
            string greeting;
            greeting = api.SayHellow(out greeting, "Hugo");
            greeting = api.SayHellow(out greeting, "Hugo");

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            greeting = api.SayHellow(out greeting, "Hugo's Wife");
            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(2, api.InvocationCount);
        }
    }
}
