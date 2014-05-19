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
        public class ComplexType
        {
            [CacheKey]
            public string Name { get; set; }

            public string Title { get; set; }

            public ComplexType(string name, string title)
            {
                Name = name;
                Title = title;
            }
        }

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

            [CacheResult]
            public virtual string MultipleArguments(string name, int age)
            {
                InvocationCount++;
                return "Hello" + name;
            }

            //[CacheResult]
            public virtual string ComplexArgument(ComplexType info)
            {
                InvocationCount++;
                return "Hello" + info.Name;
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
        [TestCategory("Cache")]
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
        [TestCategory("Cache")]
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
        [TestCategory("Cache")]
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

        [TestMethod]
        [TestCategory("Cache")]
        public void MutlpleArgumentCacheTest()
        {
            var api = Container.Resolve<NeedToCache>();
            string greeting = api.MultipleArguments("Hugo", 10);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            greeting = api.MultipleArguments("Hugo", 10);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            greeting = api.MultipleArguments("Hugo", 11);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(2, api.InvocationCount);

            greeting = api.MultipleArguments("Hugo's Wife", 10);

            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(3, api.InvocationCount);

            greeting = api.MultipleArguments("Hugo's Wife", 11);

            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(4, api.InvocationCount);
        }

        [TestMethod]
        [TestCategory("Cache")]
        public void ComplexArgumentCacheTest()
        {
            var api = Container.Resolve<NeedToCache>();
            var info = new ComplexType("Hugo", "Developer");
            var greeting = api.ComplexArgument(info);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            info = new ComplexType("Hugo", "Developer");
            greeting = api.ComplexArgument(info);

            Assert.AreEqual("HelloHugo", greeting);
            Assert.AreEqual(1, api.InvocationCount);

            info.Name = "Hugo's Wife";
            greeting = api.ComplexArgument(info);

            Assert.AreEqual("HelloHugo's Wife", greeting);
            Assert.AreEqual(2, api.InvocationCount);
        }
    }
}
