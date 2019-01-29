using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Connected.Common.LogManagement;
using Infrastructure.AspectOriented.Aspects;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    [TestClass]
    public class LogAspectTests
    {
        public interface ILogger
        {
            void Log();
        }

        public class Logger : ILogger
        {
            [LogAspect]
            public void Log()
            {
                Console.WriteLine("Sample log.");
            }
        }

        [TestMethod]
        public void TestLoggingInterfaceResolving()
        {
            IoCManager.Instance.Register<ILogger, Logger>();
            ILogger logger = IoCManager.Instance.ResolveIfRegistered<ILogger>();

            logger.Log();
        }

        [TestMethod]
        public void TestLoggingInterfaceResolvingNamed()
        {
            IoCManager.Instance.Register<ILogger, Logger>("test");
            ILogger logger = IoCManager.Instance.ResolveIfRegistered<ILogger>("test");

            logger.Log();
        }

    }
}
