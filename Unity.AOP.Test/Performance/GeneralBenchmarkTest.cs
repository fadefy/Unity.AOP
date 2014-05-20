using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.AOP.Caching;
using Unity.AOP.ExcptionHandling;
using Unity.AOP.HangingDetection;
using Unity.AOP.Logging;
using Unity.AOP.Test.Framework;
using Unity.AOP.Threading;

namespace Unity.AOP.Test.Performance
{
    [TestClass]
    public class GeneralBenchmarkTest : TestBase
    {
        public class API
        {
            [CacheResult(Order = 5)]
            [HandleException(Order = 4)]
            [DetectHanging(Order = 3)]
            [LoggingInvocation(Order = 2)]
            [ThreadDispatching(Order = 1, TargetThreadType = ThreadType.Current)]
            public virtual string GetValue([ExcludeFromLog]string key)
            {
                return new Dictionary<string, string>() { { "", "Value" } } [key];
            }
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<API>(new PerResolveLifetimeManager(),
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());
        }

        [TestMethod]
        public void FullCallHandlerPenaltyTest()
        {
            var invocationTimes = 10000;
            var api = Container.Resolve<API>();
            api.GetValue(String.Empty);

            var stopwatch = Stopwatch.StartNew();
            for (var i =0; i < invocationTimes; i++)
            {
                api.GetValue(string.Empty);
            }
            stopwatch.Stop();

            Trace.WriteLine(String.Format("FullCallHandler used {0} ms", stopwatch.ElapsedMilliseconds));

            api = new API();
            api.GetValue(String.Empty);

            stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < invocationTimes; i++)
            {
                api.GetValue(string.Empty);
            }
            stopwatch.Stop();

            Trace.WriteLine(String.Format("Direct call used {0} ms", stopwatch.ElapsedMilliseconds));
        }
    }
}
