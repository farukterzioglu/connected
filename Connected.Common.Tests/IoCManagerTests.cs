using System;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Connected.Common.Tests
{
    [TestClass]
    public class IoCManagerTests
    {
        public interface IPerson
        {
            Guid Id { get; set; }
            string Name { get; set; }
        }

        public class PersonType1 : IPerson
        {
            public Guid Id { get; set; }

            public PersonType1()
            {
                Name = "PersonType1";

                Id = Guid.NewGuid();
                Console.WriteLine("Created PersonType1 instance.");
            }

            public string Name { get; set; }
        }

        public class PersonType2 : IPerson
        {
            public Guid Id { get; set; }

            public PersonType2()
            {
                Name = "PersonType2";
            }

            public string Name { get; set; }
        }

        public class PersonRecorder
        {
            public string PersonName { get; set; }

            public PersonRecorder(IPerson person)
            {
                PersonName = person.Name;
                Console.WriteLine(person.Name);
            }
        }

        [TestMethod]
        public void RegisterResolveInterface()
        {
            //Register type 
            IoCManager.Instance.Register<IPerson, PersonType1>();

            //Resolve type
            IPerson person = IoCManager.Instance.ResolveIfRegistered<IPerson>();

            //Assert
            Assert.IsTrue(person.GetType() == typeof(PersonType1));
            Assert.AreEqual(person.Name, "PersonType1");
        }

        [TestMethod]
        public void RegisterResolveInterfaceNamed()
        {
            //Register type 
            IoCManager.Instance.Register<IPerson, PersonType1>("Person1");
            IoCManager.Instance.Register<IPerson, PersonType2>("Person2");

            //Resolve type
            IPerson person = IoCManager.Instance.ResolveIfRegistered<IPerson>("Person1");

            //Assert
            Assert.IsTrue(person.GetType() == typeof(PersonType1));
            Assert.AreEqual(person.Name, "PersonType1");
        }

        [TestMethod]
        public void RegisterResolvePerRequest()
        {
            //Register type 
            IoCManager.Instance.Register<PersonType1>(IoCLifeTimeType.PerRequest);

            //Resolve type
            IPerson person = IoCManager.Instance.ResolveIfRegistered<PersonType1>();
            IPerson person1 = IoCManager.Instance.ResolveIfRegistered<PersonType1>();

            Assert.AreNotEqual(person.Id, person1.Id);

            PersonType1 person11 = IoCManager.Instance.ResolveIfRegistered(typeof(PersonType1)) as PersonType1;
            PersonType1 person12 = IoCManager.Instance.ResolveIfRegistered(typeof(PersonType1)) as PersonType1;
            Assert.AreNotEqual(person11.Id, person12.Id);
        }

        [TestMethod]
        public void RegisterResolvePerThread()
        {
            //Register type 
            IoCManager.Instance.Register<PersonType1>(IoCLifeTimeType.PerThread);

            //Resolve type
            IPerson person = IoCManager.Instance.ResolveIfRegistered<PersonType1>();
            IPerson person1 = IoCManager.Instance.ResolveIfRegistered<PersonType1>();

            Assert.AreEqual(person.Id, person1.Id);

            PersonType1 person11 = IoCManager.Instance.ResolveIfRegistered(typeof(PersonType1)) as PersonType1;
            PersonType1 person12 = IoCManager.Instance.ResolveIfRegistered(typeof(PersonType1)) as PersonType1;
            Assert.AreEqual(person11.Id, person12.Id);
        }

        [TestMethod]
        public void ResolveDependencies()
        {
            IoCManager.Instance.Register<IPerson, PersonType1>();
            //PersonType1 person = IoCManager.Instance.Resolve<PersonType1>();

            PersonRecorder recorder = IoCManager.Instance.ResolveDependencies<PersonRecorder>() ;
            Assert.IsTrue(recorder.PersonName == "PersonType1");
        }

        [TestMethod]
        public void ResolveInstance()
        {
            //Register type & instance
            PersonType1 personType1 = new PersonType1 {Name = "Faruk"};
            IoCManager.Instance.RegisterInstance(personType1);
            IoCManager.Instance.Register<IPerson, PersonType1>();

            //Resolve
            PersonRecorder recorder =
                IoCManager.Instance.ResolveDependencies<PersonRecorder>();

            //Assert
            Assert.IsTrue(recorder.PersonName == "Faruk");
        }

        [TestMethod]
        public void ResolveInstanceContainerControllled()
        {
            //Register type & instance
            //PersonType1 personType1 = new PersonType1 {Name = "Faruk"};
            //IoCManager.Instance.RegisterInstance(personType1);
            IoCManager.Instance.Register<IPerson, PersonType1>(IoCLifeTimeType.ContainerControllled);

            PersonType1 person1 = IoCManager.Instance.ResolveIfRegistered<IPerson>() as PersonType1;
            person1.Name = "Faruk";

            //Resolve
            PersonRecorder recorder =
                IoCManager.Instance.ResolveDependencies<PersonRecorder>();

            //Assert
            Assert.IsTrue(recorder.PersonName == "Faruk");
        }

        [TestMethod]
        public void ResolveInstanceWithUnity()
        {
            UnityContainer container = new UnityContainer();

            //Register type & instance
            PersonType1 personType1 = new PersonType1 { Name = "Faruk" };
            container.RegisterInstance(personType1);
            container.RegisterType<IPerson, PersonType1>();

            //Resolve
            PersonRecorder recorder =
                container.Resolve<PersonRecorder>();

            //Assert
            Assert.IsTrue(recorder.PersonName == "Faruk");
        }
    }
}
