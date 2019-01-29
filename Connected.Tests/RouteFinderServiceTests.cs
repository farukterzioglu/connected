using System;
using System.Threading.Tasks;
using Connected.Common;
using Connected.ModuleManager;
using Connected.Schemas;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Connected.ModuleManager.ConnectedEnvironment;

namespace Connected.Tests
{
    [TestClass]
    public class RouteFinderServiceTests
    {
        private Connected.Schemas.IMessageStorage _messageStorage;
        //private Connected.DAL.Configuration.Repositories.IConnectedConfDBRepository _configurationDB;

        private Connected.Schemas.IRouteFinderService _service;

        [TestInitialize]
        public void Init()
        {
            ConnectedModuleManager.Instance.RegisterModulesForIoC(Environment.Production);

            //Select message queueu entities
            _messageStorage = IoCManager.Instance.ResolveIfRegistered<IMessageStorage>();
            //_configurationDB = new Connected.DAL.Configuration.Repositories.EF.ConnectedConfDBRepository();

            _service = IoCManager.Instance.ResolveIfRegistered<RouteFinder.RouteFinderService>(); //new RouteFinder.RouteFinderService(_messageStorage, _configurationDB);
        }

        [TestMethod]
        public void StartRouteFinderService()
        {
            var task = _service.StartRouteFinderService();
            //Task.Delay(45000).Wait();

            _service.Close();

            //Assert.IsTrue(task.IsCanceled);
        }

        [TestMethod]
        public void RouteMessages()
        {
            var result = _service.RouteMessages();
            Assert.IsTrue(result);
        }
    }
}
