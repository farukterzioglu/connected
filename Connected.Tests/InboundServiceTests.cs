using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Connected.Common;
using Connected.Common.SerializationHelper;
using Connected.DAL.Configuration.Repositories;
using Connected.Inbound;
using Connected.ModuleManager;
using Connected.Schemas;
using Connected.Schemas.Common;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Connected.ModuleManager.ConnectedEnvironment;

namespace Connected.Tests
{
    [TestClass]
    public class InboundServiceTests
    {
        private Connected.Schemas.Common.IInboundService _service;
        private IMessageStorage _messageStorage;

        [TestInitialize]
        public void HostInboundService()
        {
            ConnectedModuleManager.Instance.RegisterModulesForIoC(Environment.Test);

            //Select message queueu entities
            _messageStorage = IoCManager.Instance.ResolveIfRegistered<IMessageStorage>();// new FakeStorage(); //_messageStorage = new MessageQueueStorage();
            ////Resolve Configuration DB Repository
            //IConnectedConfDBRepository connectedConfDBRepository = IoCManager.Instance.Resolve<IConnectedConfDBRepository>();
            //Host Inbound service            
            //_service = new InboundService(_messageStorage, connectedConfDBRepository);

            //Host Inbound service
            _service = IoCManager.Instance.ResolveIfRegistered<InboundService>();

            //TODO : Implement wcf service call function for Inbound service
            //TODO : them host service here, call this instance
            //try
            //{
            //    ServiceHost serviceHost = new ServiceHost(_service);
            //    serviceHost.Open();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        [TestMethod]
        public void TestProcessTransferMessage()
        {
            //Message Storage
            
            int messageCount = _messageStorage.GetAllMessages().Count();

            var newTransferMesage = new TransferMessage()
            {
                Header = new TransferMessageHeader()
                {
                    MessageType = "Unit Test Message",
                    Source = "Unit Test",
                    Status = "Testing",
                    TimeStamp = DateTime.Now.ToString()
                },
                Body = SerializationHelper.SerializeObjectToXml<List<string>>(
                    new List<string>(){ "<Test></Test>", "<Test></Test>"} 
                ).DocumentElement
            };

            var response = _service.ProcessTransferMessage(newTransferMesage, "UnitTestSendAdapter");

            Assert.IsTrue(response.Header.Status == "Received");

            int messageCountAfter = _messageStorage.GetAllMessages().Count();

            Assert.IsTrue(messageCountAfter > messageCount);
        }

        [TestMethod]
        public void TestHostedInboundService()
        {
            var newTransferMesage = new TransferMessage()
            {
                Header = new TransferMessageHeader()
                {
                    Destination = "Command Center",
                    MessageType = "Unit Test Message",
                    Source = "UnitTestSendAdapter",
                    Status = "Testing",
                    TimeStamp = DateTime.Now.ToString()
                },
                Body = SerializationHelper.SerializeObjectToXml<List<string>>(
                    new List<string>(){ "Test 1", "Test 2"} 
                ).DocumentElement
            };

            var response = 
                Connected.Common.ClientWCFServiceInvokerUtil.CallInboundWcfService(
                "ConnectInBoundHttpEndPoint", 
                newTransferMesage, 
                "Counter Send Adapter");

            Assert.IsTrue(response.Header.Status == "Received");
        }

        #region obsolute codes
        ///// <summary>
        ///// Obsolute, Instead of mocking DbContext, use FakeRepository
        ///// </summary>
        ///// <param name="mockedConfContext"></param>
        ///// <param name="mockedMessageQueueDB"></param>
        //private void GetMockedDbContextes(out Mock<ConnectedConfEntities> mockedConfContext, out Mock<ConnectedMessageQueueEntities> mockedMessageQueueDB)
        //{
        //    //Mock Configuration DB
        //    var mockAdapterBasic = new Mock<DbSet<AdapterBasic>>();
        //    var mockAdapterMessageType = new Mock<DbSet<AdapterMessageType>>();
        //    var mockAdapterType = new Mock<DbSet<AdapterTypeDIM>>();
        //    var mockReceiveAdapterDetails = new Mock<DbSet<ReceiveAdapterDetails>>();
        //    var mockMessageSubscriptionDetails = new Mock<DbSet<MessageSubscriptionDetails>>();

        //    ////Insert data

        //    //Adapter types             
        //    var adapterTypes = new List<AdapterTypeDIM>()
        //    {
        //        new AdapterTypeDIM()
        //        {
        //            AdapterTypeId = 1,
        //            AdapterType = "Send Adapter",
        //            CreationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now
        //        },
        //        new AdapterTypeDIM()
        //        {
        //            AdapterTypeId = 2,
        //            AdapterType = "Receive Adapter",
        //            CreationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now
        //        }
        //    }.AsQueryable();
        //    mockAdapterType.As<IQueryable<AdapterTypeDIM>>().Setup(x => x.Provider).Returns(adapterTypes.Provider);
        //    mockAdapterType.As<IQueryable<AdapterTypeDIM>>().Setup(x => x.Expression).Returns(adapterTypes.Expression);
        //    mockAdapterType.As<IQueryable<AdapterTypeDIM>>().Setup(x => x.ElementType).Returns(adapterTypes.ElementType);
        //    mockAdapterType.As<IQueryable<AdapterTypeDIM>>().Setup(x => x.GetEnumerator()).Returns(adapterTypes.GetEnumerator());
        //    mockAdapterType.Setup(x => x.AsNoTracking()).Returns(mockAdapterType.Object);


        //    //Adapters 
        //    var adapterBasics = new List<AdapterBasic>()
        //    {
        //        new AdapterBasic()
        //        {
        //            AdapterId = 1,
        //            AdapterName = "Mocked Send Adapter",
        //            // ReSharper disable once PossibleNullReferenceException
        //            AdapterTypeId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Send Adapter").AdapterTypeId,
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now,
        //            IsActive = true
        //        },
        //        new AdapterBasic()
        //        {
        //            AdapterId = 2,
        //            AdapterName = "Mocked Receive Adapter",
        //            // ReSharper disable once PossibleNullReferenceException
        //            AdapterTypeId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Receive Adapter").AdapterTypeId,
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now,
        //            IsActive = true
        //        }
        //    }.AsQueryable();
        //    mockAdapterBasic.As<IQueryable<AdapterBasic>>().Setup(x => x.Provider).Returns(adapterBasics.Provider);
        //    mockAdapterBasic.As<IQueryable<AdapterBasic>>().Setup(x => x.ElementType).Returns(adapterBasics.ElementType);
        //    mockAdapterBasic.As<IQueryable<AdapterBasic>>().Setup(x => x.Expression).Returns(adapterBasics.Expression);
        //    mockAdapterBasic.As<IQueryable<AdapterBasic>>().Setup(x => x.GetEnumerator()).Returns(adapterBasics.GetEnumerator());
        //    mockAdapterBasic.Setup(x => x.AsNoTracking()).Returns(mockAdapterBasic.Object);

        //    //mockAdapterBasic.Setup(x => x.Include(y => y.ReceiveAdapterDetails)).Returns(mockAdapterBasic.Object.Include( x=> x.ReceiveAdapterDetails));

        //    //Adapter Details
        //    var receiveAdapterDetail = new List<ReceiveAdapterDetails>()
        //    {
        //        new ReceiveAdapterDetails()
        //        {
        //            AdapterId = 2,
        //            AdapterServiceURI = "http://localhost:80/Test",
        //            IsWCFService = true,
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now,
        //            IsActive = true
        //        }
        //    }.AsQueryable();
        //    mockReceiveAdapterDetails.As<IQueryable<ReceiveAdapterDetails>>().Setup(x => x.Provider).Returns(receiveAdapterDetail.Provider);
        //    mockReceiveAdapterDetails.As<IQueryable<ReceiveAdapterDetails>>().Setup(x => x.ElementType).Returns(receiveAdapterDetail.ElementType);
        //    mockReceiveAdapterDetails.As<IQueryable<ReceiveAdapterDetails>>().Setup(x => x.Expression).Returns(receiveAdapterDetail.Expression);
        //    mockReceiveAdapterDetails.As<IQueryable<ReceiveAdapterDetails>>().Setup(x => x.GetEnumerator()).Returns(receiveAdapterDetail.GetEnumerator());
        //    mockReceiveAdapterDetails.Setup(x => x.AsNoTracking()).Returns(mockReceiveAdapterDetails.Object);

        //    //Message Type
        //    var messageTypes = new List<MessageType>()
        //    {
        //        new MessageType()
        //        {
        //            MessageTypeId = 1,
        //            MessageType1 = "Unit Test Message",
        //            MessageSchema = "c:/TestSchema.txt",
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now
        //        }
        //    }.AsQueryable();

        //    var mockMessageType = new Mock<DbSet<MessageType>>();
        //    mockMessageType.As<IQueryable<MessageType>>().Setup(x => x.Provider).Returns(messageTypes.Provider);
        //    mockMessageType.As<IQueryable<MessageType>>().Setup(x => x.ElementType).Returns(messageTypes.ElementType);
        //    mockMessageType.As<IQueryable<MessageType>>().Setup(x => x.Expression).Returns(messageTypes.Expression);
        //    mockMessageType.As<IQueryable<MessageType>>().Setup(x => x.GetEnumerator()).Returns(messageTypes.GetEnumerator());
        //    mockMessageType.Setup(x => x.AsNoTracking()).Returns(mockMessageType.Object);

        //    //Subscription
        //    var adapterMessageType = new List<AdapterMessageType>()
        //    {
        //        new AdapterMessageType()
        //        {
        //            // ReSharper disable once PossibleNullReferenceException
        //            AdapterId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Send Adapter").AdapterTypeId,
        //            // ReSharper disable once PossibleNullReferenceException
        //            MessageTypeId = messageTypes.FirstOrDefault().MessageTypeId,
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now
        //        }
        //    }.AsQueryable();
        //    mockAdapterMessageType.As<IQueryable<AdapterMessageType>>().Setup(x => x.Provider).Returns(adapterMessageType.Provider);
        //    mockAdapterMessageType.As<IQueryable<AdapterMessageType>>().Setup(x => x.ElementType).Returns(adapterMessageType.ElementType);
        //    mockAdapterMessageType.As<IQueryable<AdapterMessageType>>().Setup(x => x.Expression).Returns(adapterMessageType.Expression);
        //    mockAdapterMessageType.As<IQueryable<AdapterMessageType>>().Setup(x => x.GetEnumerator()).Returns(adapterMessageType.GetEnumerator());
        //    mockAdapterMessageType.Setup(x => x.AsNoTracking()).Returns(mockAdapterMessageType.Object);

        //    var messageSubscriptionDetails = new List<MessageSubscriptionDetails>()
        //    {
        //        new MessageSubscriptionDetails()
        //        {
        //            // ReSharper disable once PossibleNullReferenceException
        //            AdapterId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Receive Adapter").AdapterTypeId,
        //            // ReSharper disable once PossibleNullReferenceException
        //            MessageTypeId = messageTypes.FirstOrDefault().MessageTypeId,
        //            RegistrationDate = DateTime.Now,
        //            ModifiedDate = DateTime.Now,
        //            IsActive = true,
        //            SubscriptionCriteria = ""
        //        }
        //    }.AsQueryable();
        //    mockMessageSubscriptionDetails.As<IQueryable<MessageSubscriptionDetails>>().Setup(x => x.Provider).Returns(messageSubscriptionDetails.Provider);
        //    mockMessageSubscriptionDetails.As<IQueryable<MessageSubscriptionDetails>>().Setup(x => x.ElementType).Returns(messageSubscriptionDetails.ElementType);
        //    mockMessageSubscriptionDetails.As<IQueryable<MessageSubscriptionDetails>>().Setup(x => x.Expression).Returns(messageSubscriptionDetails.Expression);
        //    mockMessageSubscriptionDetails.As<IQueryable<MessageSubscriptionDetails>>().Setup(x => x.GetEnumerator()).Returns(messageSubscriptionDetails.GetEnumerator());
        //    mockMessageSubscriptionDetails.Setup(x => x.AsNoTracking()).Returns(mockMessageSubscriptionDetails.Object);

        //    //Mock DbContext
        //    mockedConfContext = new Mock<ConnectedConfEntities>(); //Context 
        //    mockedConfContext.Setup(x => x.MessageType).Returns(mockMessageType.Object);
        //    mockedConfContext.Setup(x => x.AdapterTypeDIM).Returns(mockAdapterType.Object);
        //    mockedConfContext.Setup(x => x.AdapterBasic).Returns(mockAdapterBasic.Object);
        //    mockedConfContext.Setup(x => x.AdapterMessageType).Returns(mockAdapterMessageType.Object);
        //    mockedConfContext.Setup(x => x.ReceiveAdapterDetails).Returns(mockReceiveAdapterDetails.Object);
        //    mockedConfContext.Setup(x => x.MessageSubscriptionDetails).Returns(mockMessageSubscriptionDetails.Object);

        //    //Mock Message Queue
        //    var messageQueueMessages = new List<Messages>()
        //    {
        //        //new Messages(){ Message = "Test"}
        //    }.AsQueryable();

        //    var mockMessages = new Mock<DbSet<Messages>>();
        //    mockMessages.As<IQueryable<Messages>>().Setup(x => x.Provider).Returns(messageQueueMessages.Provider);
        //    mockMessages.As<IQueryable<Messages>>().Setup(x => x.ElementType).Returns(messageQueueMessages.ElementType);
        //    mockMessages.As<IQueryable<Messages>>().Setup(x => x.Expression).Returns(messageQueueMessages.Expression);
        //    mockMessages.As<IQueryable<Messages>>().Setup(x => x.GetEnumerator()).Returns(messageQueueMessages.GetEnumerator());
        //    //mockMessages.Setup(x => x.Add());
        //    mockMessages.Setup(x => x.AsNoTracking()).Returns(mockMessages.Object);

        //    mockedMessageQueueDB = new Mock<ConnectedMessageQueueEntities>();
        //    mockedMessageQueueDB.Setup(x => x.Messages).Returns(mockMessages.Object);
        //}

        #endregion
    }
}