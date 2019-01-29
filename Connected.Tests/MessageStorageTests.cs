using System;
using System.Linq;
using Connected.Schemas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Tests
{
    public abstract class MessageStorageTests
    {
        public Connected.Schemas.IMessageStorage MessageStorage;

        protected MessageStorageTests(Connected.Schemas.IMessageStorage messageStorage)
        {
            MessageStorage = messageStorage;
        }

        [TestMethod]
        public void GetMessagesToBeDispatched()
        {
            var list = MessageStorage.GetMessagesToBeDispatched(10, 2);
            Assert.IsTrue(list.Any());
        }
    }

    [TestClass]
    public class MessageQueueStorageTests : MessageStorageTests
    {
        private static readonly IMessageStorage MessageStorageSelected =
            new Connected.MessageStorage.MessageQueueStorage();

        public MessageQueueStorageTests()
            : base(MessageStorageSelected)
        {
        }
    }
}
