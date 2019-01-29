using System;
using System.Collections.Generic;
using System.Linq;
using Connected.Common;
using Connected.DAL.Core;
using Connected.Tests.TestAssets;

namespace Connected.DAL.DbTriggers.SampleTriggerReader
{
    public class TriggerReader
    {
        private readonly int _triggerTypesCount = 1;
        private readonly int _dbTriggerCount = 3;

        public  int DBTriggerCount
        {
            get { return _dbTriggerCount; }
        }

        public  int TriggerTypesCount
        {
            get { return _triggerTypesCount; }
        }

        private class DBTrigger1 : DBTrigger
        {
            public DBTrigger1()
            {
                Console.WriteLine("DBTrigger1 instance created");
            }
            public override Type ApplyOn
            {
                get { return typeof(DbEntityA); }
            }
            public override void AfterInsert()
            {
                Console.WriteLine("DBTrigger1-AfterInsert");
            }
            public override void BeforeInsert()
            {
                Console.WriteLine("DBTrigger1-BeforeInsert");
            }
        }
        private class DBTrigger2 : DBTrigger
        {
            public DBTrigger2()
            {
                Console.WriteLine("DBTrigger2 instance created");
            }
            public override Type ApplyOn
            {
                get { return typeof (DbEntityB); }
            }
            public override void AfterInsert()
            {
                Console.WriteLine("DBTrigger2-AfterInsert");
            }
            public override void BeforeInsert()
            {
                Console.WriteLine("DBTrigger2-BeforeInsert");
            }
        }
        private class DBTrigger3 : DBTrigger
        {
            public override Type ApplyOn
            {
                get { return typeof(DbEntityB); }
            }

            public override void BeforeUpdate()
            {
                Console.WriteLine("DBTrigger3-BeforeUpdate");
            }
            public override void AfterInsert()
            {
                Console.WriteLine("DBTrigger3-AfterInsert");
            }
            public override void BeforeInsert()
            {
                Console.WriteLine("DBTrigger3-BeforeInsert");
            }
        }

        public  List<Type> ReadTriggerList()
        {
            return new List<Type>()
            {
                typeof(DBTrigger1),
                typeof(DBTrigger2),
                typeof(DBTrigger3)
            };
        }
    }
}
