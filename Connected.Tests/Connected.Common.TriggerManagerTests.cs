using System;
using System.Collections.Generic;
using System.Linq;
using Connected.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    //Generic Trigger Manager Tests
    [TestClass]
    public class TriggerManagerTests
    {
        private interface IDbTrigger
        {
            Type ApplyOn { get; }
            void AfterInsert();
            void BeforeUpdate();
        }
        private abstract class DBTrigger : ITrigger, IDbTrigger
        {
            protected DBTrigger()
            {
                //string temp = this.GetType().GetMethods().
                //    Where(x => x.DeclaringType == this.GetType()).
                //    Select(x => x.Name).
                //    Aggregate("", (current, str) => current + str + ", ");

                //Console.WriteLine("Class initialized : " + this.GetType().Name);
                ////Console.WriteLine("Applies on : " + ApplyOn.Name);
                //Console.WriteLine("Methods : " + temp );
                //Console.WriteLine();
            }

            public Type TriggerType
            {
                get { return typeof(DBTrigger); }
            }

            public abstract Type ApplyOn { get; }

            public virtual void AfterInsert()
            {
            }
            public virtual void BeforeUpdate()
            {
                Console.WriteLine("DBTrigger-BeforeUpdate");
            }

            public void Dispose()
            {
            }
        }
        private abstract class WindowsServiceTrigger : ITrigger
        {
            public virtual void BeforeStart()
            {
                Console.WriteLine("WindowsServiceTrigger-BeforeStart");
            }

            public Type TriggerType
            {
                get { return typeof(WindowsServiceTrigger); }
            }

            public void Dispose()
            {
            }
        }

        private static class TriggerReader
        {
            private static readonly int _triggerTypesCount = 2;
            private static readonly int _triggerCount = 5;
            private static readonly int _dbTriggerCount = 3;
            private static readonly int _wsTriggerCount  = 2;

            public static int TriggerCount
            {
                get { return _triggerCount; }
            }

            public static int DBTriggerCount
            {
                get { return _dbTriggerCount; }
            }

            public static int WSTriggerCount
            {
                get { return _wsTriggerCount; }
            }

            public static int TriggerTypesCount
            {
                get { return _triggerTypesCount; }
            }

            private class DBTrigger1 : DBTrigger
            {
                public DBTrigger1()
                {
                    Console.WriteLine("DBTrigger1 instance created");
                }

                private class A
                {
                }

                public override Type ApplyOn
                {
                    get { return typeof(A); }
                }
            }
            private class DBTrigger2 : DBTrigger
            {
                public override Type ApplyOn
                {
                    get { throw new NotImplementedException(); }
                }
            }
            private class DBTrigger3 : DBTrigger
            {
                public override Type ApplyOn
                {
                    get { throw new NotImplementedException(); }
                }

                public override void BeforeUpdate()
                {
                    Console.WriteLine("DBTrigger3-BeforeUpdate");
                }
            }
            private class WindowsServiceTrigger1 : WindowsServiceTrigger
            {
            }
            private class WindowsServiceTrigger2 : WindowsServiceTrigger
            {
                public override void BeforeStart()
                {
                    Console.WriteLine("WindowsServiceTrigger2-BeforeStart");
                }
            }

            public static List<Type> ReadTriggerList()
            {
                return new List<Type>()
            {
                typeof(DBTrigger1),
                typeof(DBTrigger2),
                typeof(DBTrigger3),
                typeof(WindowsServiceTrigger1),
                typeof(WindowsServiceTrigger2),
            };
            }
        }

        [TestInitialize]
        public void TestInit()
        {
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

            Assert.AreEqual(first, second, TriggerReader.TriggerCount);
        }

        [TestMethod]
        public void InitializeTriggerManagerTwoTimesWithTwoTypes()
        {
            TriggerManager.Instance.Initialize<DBTrigger>(TriggerReader.ReadTriggerList());
            TriggerManager.Instance.Initialize<WindowsServiceTrigger>(TriggerReader.ReadTriggerList());

            Type type = typeof (DBTrigger);
            var dbTriggers = TriggerManager.Instance.GetTriggers<DBTrigger>();
            Assert.AreEqual(dbTriggers.Count, TriggerReader.DBTriggerCount);

            var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            Assert.AreEqual(wsTriggers.Count, TriggerReader.WSTriggerCount);

            //All triggers
            var allTriggers = TriggerManager.Instance.GetTriggers();
            Assert.AreEqual(
                allTriggers.Values.Sum(list => list.Count),
                TriggerReader.TriggerCount);

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
                TriggerReader.TriggerCount);

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

            var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            Assert.AreEqual(wsTriggers.Count, TriggerReader.WSTriggerCount);

            var allTriggers = TriggerManager.Instance.GetTriggers();
            Assert.AreEqual(
                allTriggers.Values.Sum(list => list.Count),
                TriggerReader.TriggerCount);
        }

        public void GetAllTriggersWithoutInit()
        {
            var triggers = TriggerManager.Instance.GetTriggers();

            Assert.AreEqual(
                triggers.Values.Sum(list => list.Count),
                TriggerReader.TriggerCount);

            Assert.AreEqual(
                triggers.Count(),
                TriggerReader.TriggerTypesCount);
        }

        [TestMethod]
        public void GetAllTriggersByTypeWithoutInit()
        {
            var dbTriggers = TriggerManager.Instance.GetTriggers<DBTrigger>();
            Assert.AreEqual(dbTriggers.Count, TriggerReader.DBTriggerCount);

            var wsTriggers = TriggerManager.Instance.GetTriggers<WindowsServiceTrigger>();
            Assert.AreEqual(wsTriggers.Count, TriggerReader.WSTriggerCount);

            var allTriggers = TriggerManager.Instance.GetTriggers();
            Assert.AreEqual(
                allTriggers.Values.Sum(list => list.Count),
                TriggerReader.TriggerCount);
        }
    }
}
