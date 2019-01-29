using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Connected.Configuration.WebApp;
using Infrastructure.AspectOriented.Aspects;
using Infrastructure.CrossCutting.IocManager;
using Infrastructure.PluginFramework.Core;
using Infrastructure.WebApps.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    public class TestController : Controller, IPluginController
    {
        private class TestPlugin : IPlugin
        {
            public List<IPluginController> ControllerList { get; set; }
            public string PluginAssemblyName
            {
                get { return "TestPlugin"; }
            }

            public List<Type> ControllerTypes { get; set; }

            public string PluginName
            {
                get { return "Test Plugin"; }
            }
        }

        public TestController ()
        {
            Plugin = new TestPlugin();
        }

        public IPlugin Plugin { get; set; }

        public string AssemblyName
        {
            get { return "Test"; }
        }

        public Type PluginType { get; private set; }
        public string Layout { get; set; }
        public string Title { get; set; }

        [LogAspect]
        [AuthorizeAspect("TestPlugin/TestController/Index", new string[] { "TestPlugin/TestController/Index" })]
        public ActionResult Index()
        {
            Console.WriteLine("TestController/Index");

            return null;
        }

        //TODO : How to set claim automatically
        [AuthorizeAspect("TestPlugin/TestController/Update", new string[] { "TestPlugin/TestController/Index" })]
        public ActionResult Update()
        {
            Console.WriteLine("TestController/Update");

            return null;
        }
    }

    [TestClass]
    public class AuthorizeAspectTests
    {
        [TestInitialize]
        public void Init()
        {
            IoCManager.Instance.Register<IUserClaimsManager, UserClaimsManager>();
        }
        //Authorization test for 'Update' ('not named' registration)
        [TestMethod]
        public void TestAuthorizeForUpdateWithInterfaceResolving()
        {
            try
            {
                IoCManager.Instance.Register<IPluginController, TestController>();
                IPluginController controller = IoCManager.Instance.ResolveIfRegistered<IPluginController>();

                //TODO : Test another action that user doesn't have claim
                var actionResult = controller.Index();

                Assert.IsTrue(false, "Update should throw NotAuthorizedException exception");
            }
            catch (NotAuthorizedException ex)
            {
                Assert.IsTrue(true);
            }
        }

        //Authorization test for 'Update' ('named' registration)
        [TestMethod]
        public void TestAuthorizeForUpdateWithNamedRegistration()
        {
            try
            {
                IoCManager.Instance.Register<IPluginController, TestController>("TestController");
                IPluginController controller = IoCManager.Instance.ResolveIfRegistered<IPluginController>("TestController");

                //TODO : Test another action that user doesn't have claim
                var actionResult = controller.Index();

                Assert.IsTrue(false, "Update should throw NotAuthorizedException exception");
            }
            catch (NotAuthorizedException ex)
            {
                Assert.IsTrue(true);
            }
        }

        //Authorization test for 'Index' ('not named' registration)
        [TestMethod]
        public void TestAuthorizeForIndexWithInterfaceResolving()
        {
            IoCManager.Instance.Register<IPluginController, TestController>();
            IPluginController controller = IoCManager.Instance.ResolveIfRegistered<IPluginController>();

            var actionResult = controller.Index();
        }

        //Authorization test for 'Index' ('named' registration)
        [TestMethod]
        public void TestAuthorizeForIndexNamedController()
        {
            IoCManager.Instance.Register<IPluginController, TestController>("TestController");
            IPluginController controller = IoCManager.Instance.ResolveIfRegistered<IPluginController>("TestController");

            var actionResult = controller.Index();
        }

    }
}
