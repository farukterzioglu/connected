using System;
using System.Globalization;
using Connected.Schemas;
using Connected.Schemas.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReceiveAdapter;

namespace UnitTests
{
    [TestClass]
    public class ReceiveAdapterServiceTests
    {
        [TestMethod]
        public void TestProcessMessage() 
        {
            var newTransferMesage = new TransferMessage()
            {
                Header = new TransferMessageHeader()
                {
                    Destination = "Receive Adapter",
                    MessageType = "Service Test Message",
                    Source = "Unit Test",
                    Status = "Testing",
                    TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                },
                Body = null
            };

            new ReceiveAdapterService().ProcessMessage(newTransferMesage);

            Assert.IsTrue(true);
        }
    }
}
