using System;
using Connected.Common.LogManagement;
using Connected.ModuleManager;
using Infrastructure.AspectOriented.Aspects;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    [TestClass]
    public class TraceAspectTests
    {
        public interface ITraceableClass
        {
            void TraceableMethod();
        }

        public class TraceableClass : ITraceableClass
        {
            [TraceAspect]
            public void TraceableMethod()
            {
                IoCManager.Instance.ResolveIfRegistered<ILoger>().WriteLog("Actual method invoke");
            }
        }

        [TestInitialize]
        public void Init()
        {
            ConnectedModuleManager.Instance.RegisterModulesForIoC(ConnectedEnvironment.Production);
        }

        [TestMethod]
        public void TestTraceableMethod()
        {
            IoCManager.Instance.Register<ITraceableClass, TraceableClass>();
            ITraceableClass traceableClass = IoCManager.Instance.ResolveIfRegistered<ITraceableClass>();

            traceableClass.TraceableMethod();
        }
    }
}
