using System;
using System.Collections.Generic;
using System.Linq;
using Connected.Common;
using Connected.DAL.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    //Generic Trigger Manager Tests
    [TestClass]
    public class TriggerManagerTests
    {
        public Connected.DAL.DbTriggers.SampleTriggerReader.TriggerReader TriggerReader;

        [TestInitialize]
        public void TestInit()
        {
            TriggerReader = new Connected.DAL.DbTriggers.SampleTriggerReader.TriggerReader();
            TriggerManager.Instance.ClearTriggers();
        }

        [TestMethod]
        public void InitializeTriggerManagerWithDLLPath()
        {
            TriggerManager.Instance.Initialize("");

            Assert.IsTrue(true);
        }
        
        [TestMethod]
        public void InitializeTriggerManagerWithDLLPathWithOnType()
        {
            TriggerManager.Instance.Initialize<DBTrigger>("");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitializeTriggerManager()
        {
            TriggerManager.Instance.Initialize(TriggerReader.ReadTriggerList());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitializeTriggerManagerWithOneType()
        {
            TriggerManager.Instance.Initialize<DBTrigger>(TriggerReader.ReadTriggerList());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitializeTriggerManagerTwoTimes()
        {
            TriggerManager.Instance.Initialize(TriggerReader.ReadTriggerList());
            var first = TriggerManager.Instance.GetTriggers().Count();
            
            TriggerManager.Instance.Initialize(TriggerReader.ReadTriggerList());
            var second = TriggerManager.Instance.GetTriggers().Count();

            Assert.AreEqual(first, second, TriggerReader.DBTriggerCount);
        }

        [TestMethod]
        public void InitializeTriggerManagerTwoTimesWithTwoTypes()
        {
            TriggerManager.Instance.Initialize<DBTrigger>(TriggerReader.ReadTriggerList());
            //TriggerManager.Instance.Initialize<WindowsServiceTrigger>(TriggerReader.ReadTriggerList());

            Type type = typeof (DBTrigger);
            var dbTriggers = TriggerManager.Instance.GetTriggers<DBTrigger>();
            Assert.AreEqual(dbTriggers.Count, TriggerReader.DBTriggerCount);

            //var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            //Assert.AreEqual(wsTriggers.Count, TriggerReader.WSTriggerCount);

            //All triggers
            var allTriggers = TriggerManager.Instance.GetTriggers();
            Assert.AreEqual(
                allTriggers.Values.Sum(list => list.Count),
                TriggerReader.DBTriggerCount);

            //Trigger Types
            var triggerTypeCount = TriggerManager.Instance.GetTriggers().Count();
            Assert.AreEqual(triggerTypeCount, TriggerReader.TriggerTypesCount);
        }
        
        [TestMethod]
        public void GetAllTriggers()
        {
            TriggerManager.Instance.Initialize(TriggerReader.ReadTriggerList());
            
            var triggers = TriggerManager.Instance.GetTriggers();

            Assert.AreEqual(
                triggers.Values.Sum(list => list.Count), 
                TriggerReader.DBTriggerCount);

            Assert.AreEqual(
                triggers.Count(),
                TriggerReader.TriggerTypesCount);
        }

        [TestMethod]
        public void GetAllTriggersByType()
        {
            TriggerManager.Instance.Initialize(TriggerReader.ReadTriggerList());

            var dbTriggers = TriggerManager.Instance.GetTriggers<DBTrigger>();
            Assert.AreEqual(dbTriggers.Count, TriggerReader.DBTriggerCount);

            //var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            //Assert.AreEqual(wsTriggers.Count, TriggerReader.WSTriggerCount);

            var allTriggers = TriggerManager.Instance.GetTriggers();
            Assert.AreEqual(
                allTriggers.Values.Sum(list => list.Count),
                TriggerReader.DBTriggerCount);
        }

        [TestMethod]
        public void GetAllTriggersWithoutInit()
        {
            var triggers = TriggerManager.Instance.GetTriggers();
            Assert.IsTrue(!triggers.Any());
        }

        [TestMethod]
        public void GetAllTriggersByTypeWithoutInit()
        {
            var dbTriggers = TriggerManager.Instance.GetTriggers<DBTrigger>();
            Assert.IsTrue(dbTriggers == null);

            //var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            //Assert.IsTrue(wsTriggers == null);
        }
    }
}
