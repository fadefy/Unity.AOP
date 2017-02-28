using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using Unity.AOP.Utilities;

namespace Unity.AOP.Test.Utilities
{
    [TestClass]
    public class ReactiveExtensionsTest
    {
        [TestMethod]
        public void ReportWithIntervalTest()
        {
            Action f = () => { Trace.WriteLine("Begin " + DateTime.Now.ToString("O")); Thread.Sleep(100); };

            var u = f.ToAsync().RepeatWithInterval(TimeSpan.FromMilliseconds(200));
            Trace.WriteLine("Done " + DateTime.Now.ToString("O"));
        }
    }
}
