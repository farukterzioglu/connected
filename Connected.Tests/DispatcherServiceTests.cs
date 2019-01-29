using System;
using System.Threading;
using System.Threading.Tasks;
using Connected.Common;
using Connected.DAL.Configuration;
using Connected.DAL.Configuration.Repositories;
using Connected.MessageStorage.Fake;
using Connected.ModuleManager;
using Connected.Schemas;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = System.Environment;

namespace Connected.Tests
{
    [TestClass]
    public class DispatcherServiceTests
    {
        private Connected.Schemas.IDispatcherService _service;

        private Connected.Schemas.IMessageStorage _messageStorage;

        [TestInitialize]
        public void Init()
        {
            ConnectedModuleManager.Instance.RegisterModulesForIoC(ConnectedEnvironment.Test);

            //Select message queueu entities
            _messageStorage = IoCManager.Instance.ResolveIfRegistered<IMessageStorage>();// new FakeStorage(); //_messageStorage = new MessageQueueStorage();

            //Resolve Configuration DB Repository
            IConnectedConfDBRepository connectedConfDBRepository = IoCManager.Instance.ResolveIfRegistered<IConnectedConfDBRepository>();

            _service = new Connected.Dispatcher.DispatcherService(_messageStorage, connectedConfDBRepository);
        }

        [TestMethod]
        public void StartService()
        {
            var task = _service.StartDispatcherService();
            Task.Delay(45000).Wait();

            _service.Close();
            
            //Assert.IsTrue(task.IsCanceled);
        }

        [TestMethod]
        public void MoveMessagesToMessagesRouterFinder()
        {
            var message = new Message()
            {
                Datetime = DateTime.Now,
                MessageText = "Test",
                MessageType = 1
            };
            _messageStorage.InsertMessage(message);

            Assert.IsTrue(_service.MoveMessagesToMessagesRouterFinder());

            Assert.IsTrue(((FakeStorage) _messageStorage).GetRouteMessages().Contains(message));
        }

        [TestMethod]
        public void PushMessages()
        {
            throw new NotImplementedException("Implement with real message storage or complete 'Fake Message Storage'");
            _service.PushMessages();
            _service.PushMessages();
            _service.PushMessages();
            _service.PushMessages();

            Assert.IsTrue(true);
        }
    }
}
