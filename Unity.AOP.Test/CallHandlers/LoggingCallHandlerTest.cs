using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.AOP.Logging;
using Unity.AOP.Mutation;
using Unity.AOP.Test.Framework;

namespace Unity.AOP.Test.CallHandlers
{
    [TestClass]
    public class LoggingCallHandlerTest : TestBase
    {
        public class ComplexType
        {
            public string Name { get; set; }

            public string Title { get; set; }

            public ComplexType(string name, string title)
            {
                Name = name;
                Title = title;
            }

            [LoggingText]
            public string LoggingTest
            {
                get { return string.Format("Name:{0}, Title:{1}", Name, Title); }
            }
        }

        public class NeedToLog
        {
            [LoggingInvocation]
            public virtual string SayHellow(string name)
            {
                return "Hello" + name;
            }

            [LoggingInvocation]
            public virtual void SayHellow(string name, out string greeting)
            {
                greeting = "Hello" + name;
            }

            [LoggingInvocation]
            public virtual string SayHellow(out string greeting, string name)
            {
                return greeting = "Hello" + name;
            }

            [LoggingInvocation]
            public virtual string MultipleArguments(string name, int age)
            {
                return "Hello" + name;
            }

            [LoggingInvocation]
            public virtual string ComplexArgument(ComplexType info)
            {
                return "Hello" + info.Name;
            }
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<NeedToLog>(new PerResolveLifetimeManager(),
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            var mutators = Container.Resolve<IAggregatedMutator>();
            mutators.SetMutator<ComplexType, string>(v => v.Name);
        }

        [TestMethod]
        public void ProxyTypeGenerationTest()
        {
            var api = Container.Resolve<NeedToLog>();
            Assert.IsNotNull(api);
            Assert.AreEqual("HelloHugo", api.SayHellow("Hugo"));
        }
    }
}
