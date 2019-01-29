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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests.AuthorizeAspectBehaviorTests
{
    public class TestController : PluginControllerBase
    {
        private class TestPlugin : IPlugin
        {
            public List<Type> ControllerTypes { get; set; }

            public string PluginName
            {
                get { return "Test Plugin"; }
            }
        }

        public override Type PluginType
        {
            get { return typeof(TestPlugin); }
        }

        public override string Layout { get; set; }
        public override string Title { get; set; }

        protected override ActionResult OnIndex()
        {
            Console.WriteLine("TestController/Index");

            return null;
        }
    }

    [TestClass]
    public class AuthorizeAspectBehaviorTests
    {
        private IPluginController _controller;

        [TestInitialize]
        public void Init()
        {
            //Register controller with name and set interception behavior
            IoCManager.Container.RegisterType<IPluginController, TestController>("TestController",
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PluginAuthorizationBehavior>());

            //Resolve
             _controller = IoCManager.Instance.ResolveIfRegistered<IPluginController>("TestController");

             IoCManager.Instance.Register<IUserClaimsManager, UserClaimsManager>();
        }

        //[TestMethod]
        //public void TestAuthorizeForUpdate()
        //{
        //    try
        //    { 
        //        var actionResult = _controller.Update();

        //        Assert.IsTrue(false, "Update should throw NotAuthorizedException exception");
        //    }
        //    catch (NotAuthorizedException ex)
        //    {
        //        Assert.IsTrue(true);
        //    }
        //}

        [TestMethod]
        public void TestAuthorizeForIndex()
        {
            var actionResult = _controller.Index();
            Assert.IsTrue(true);
        }
    }
}
