using System;
using System.Collections.Generic;
using System.Linq;
using Connected.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Tests
{
    [TestClass]
    public class SerializerHelperTest
    {
        [TestMethod]
        public void SerializeObjectToXml()
        {
            List<Activity> activityList = new Connected.DAL.Repositories.SampleActivityRepository().FindAll().ToList();

            var outerXML =
                Connected.Common.SerializationHelper.SerializationHelper.SerializeObjectToXml<List<Activity>>(
                    activityList).DocumentElement.OuterXml;

            Assert.IsTrue(!string.IsNullOrEmpty(outerXML));
        }
    }
}
