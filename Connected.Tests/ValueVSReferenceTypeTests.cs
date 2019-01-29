using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Tests
{
    [TestClass]
    public class ValueVsReferenceTypeTests
    {
        private class TestClass
        {
            private int _amount;
            public int PublicField = 55;

            public TestClass(int amount)
            {
                _amount = amount;
            }

            public int Amount {
                get { return _amount; }
                set { _amount = value > 0 ? value : 0; }
            }

            public int AddToParameter(int parameter)
            {
                return parameter + _amount;
            }

            private int _privateField = 99;
            public int PublicProperty
            {
                get { return _privateField; }
                set { _privateField = value; }
            }
        }

        private void ReferenceTypeManipulator(TestClass testClass)
        {
            int stackedValue = 10;

            testClass.Amount = 100;
        }

        private void FieldManipulatorRef(ref int field)
        {
            field *= 2;
        }

        private void FieldManipulatorOut(out int field)
        {
            //Initialized for the first time, and 'field' should be initialized
            field = 2;
        }

        [TestMethod]
        public void TestReferenceTypes()
        {
            const int testSubject = 19;

            TestClass testClass; // 'testClass' reference is being stored in the stack
            testClass = new TestClass(10);

            Console.WriteLine("Before : " + testSubject + " ->" + testClass.AddToParameter(testSubject));
            ReferenceTypeManipulator(testClass);
            Console.WriteLine("After : " + testSubject + " ->" + testClass.AddToParameter(testSubject));

            //Send field as reference
            Console.WriteLine("Before : " + testClass.PublicField);
            FieldManipulatorRef(ref testClass.PublicField);
            Console.WriteLine("After : " + testClass.PublicField);

            //Send field as reference
            int notInitializedYet;
            Console.WriteLine("Before : " + testClass.PublicField);
            FieldManipulatorOut(out notInitializedYet);
            Console.WriteLine("After : " + testClass.PublicField);

            ////Can't send property as reference
            //Console.WriteLine("Before : " + testClass.PublicProperty);
            //FieldManipulator(ref testClass.PublicProperty);
            //Console.WriteLine("After : " + testClass.PublicProperty);
        }
    }
}
