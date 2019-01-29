using System;
using System.Collections.Generic;
using Connected.Common.SerializationHelper;
using Connected.Schemas;
using Connected.Schemas.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Schremas
{
    [TestClass]
    public class SchemasTest
    {
        [TestMethod]
        public void SerializeDeserializeTransferMessage()
        {
            var transferMessage = new TransferMessage()
            {
                Header = new TransferMessageHeader() 
                {
                    Destination = "Test Destination",
                    Source = "Test Source",
                    Status = "Testing",
                    TimeStamp = DateTime.Now.ToString()
                },
                Body = SerializationHelper.SerializeObjectToXml<List<string>>(new List<string>(){"Test body message"}).DocumentElement
            };

            string transferMessageSerialized = SerializationHelper.SerializeObjectToXml<TransferMessage>(transferMessage).InnerXml;
            TransferMessage transferMessageDeserialized = (TransferMessage) SerializationHelper.DeSerializeFromString<TransferMessage>(transferMessageSerialized);

            if (string.IsNullOrWhiteSpace(transferMessageSerialized) && transferMessageDeserialized == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
    }
}
