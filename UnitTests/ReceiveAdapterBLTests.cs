using System;
using System.Collections.Generic;
using System.Configuration;
using Connected.DAL;
using Connected.DAL.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReceiveAdapterBussinessLayer;

namespace UnitTests
{
    //TODO : insert Assert controls 

    [TestClass]
    //Central Command Service Tests
    public class ReceiveAdapterBLTests
    {
        private string _newItemOriginalId;
        private string _registrationKey;
                
        private readonly SampleData4ReceiveAdapter _sampleData;

        //private ReceiveAdapterBL receiveAdapterBL;
        public ReceiveAdapterBLTests() 
        {
            //Set new originalId
            _newItemOriginalId = Guid.NewGuid().ToString();

            //Read registraion key
            _registrationKey = ConfigurationManager.AppSettings["registrationKey"] ?? null;

            //Set bussiness layer (Ninject) 
            //var kernel = new StandardKernel(new ReceiveAdapter.Bindings("EF"));
            //receiveAdapterBL = kernel.Get<ReceiveAdapterBL.ReceiveAdapterBL>();
            
            //Set sample data class
            _sampleData = new SampleData4ReceiveAdapter();
        }

        [TestMethod]
        public void RegisterNewItem()
        {
            try
            {
                //Arrange
                //Mocked DAL
                Mock<IReceiveAdapterRepository> mockedDal = new Mock<IReceiveAdapterRepository>();
                mockedDal.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns( 1);
                mockedDal.Setup(x => x.RegisterItem(null)).Throws<NullReferenceException>();

                ////Bussiness layer with real implementation of DAL
                //ReceiveAdapterBL bl = new ReceiveAdapterBL(new ReceiveAdapterDAL.EF.ReceiveAdapterDALEF( DAL.Repositories.EF.DataContextFactory.GetDataContext() ));
                //receiveAdapterBL.ReceiveActivityList(new List<Activity>() { sampleData.NewItem() });
                //Assert.IsTrue(true);

                //Bussiness layer with mocked implementation of DAL
                IReceiveAdapterRepository dal = mockedDal.Object;
                ReceiveAdapterBL bl = new ReceiveAdapterBL(dal, _registrationKey);

                //Act
                bl.ReceiveActivityList(new List<Activity>() { _sampleData.NewItem() });

                //Assert
                mockedDal.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Once);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterMultipleNewItem()
        {
            try
            {
                //Arrange
                //Mocked DAL
                Mock<IReceiveAdapterRepository> mockedDal = new Mock<IReceiveAdapterRepository>();
                mockedDal.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
                mockedDal.Setup(x => x.RegisterItem(null)).Throws<NullReferenceException>();

                //Bussiness layer with mocked implementation of DAL
                IReceiveAdapterRepository dal = mockedDal.Object;
                ReceiveAdapterBL bl = new ReceiveAdapterBL(dal, _registrationKey);

                //Act
                var list = _sampleData.MultipleNewItem();
                bl.ReceiveActivityList(list);

                //Assert
                mockedDal.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Exactly( list.Count ));
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterNewItemWithInvalidKey()
        {
            //Arrange
            Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
            mockedDAL.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
            ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);

            //Act
            bl.ReceiveActivityList(new List<Activity>() { _sampleData.NewItemWithInvalidKey() });

            //Assert
            mockedDAL.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Never);
            Assert.IsTrue(true);

            //TODO : Think about how to test behavior of BL, like logging invalid reading
            //TODO : Apply this to other test methods
        }

        [TestMethod]
        public void RegisterNewItemWithOneValidKeyAndOneInvalidKey()
        {
            //Arrange
            Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
            mockedDAL.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
            ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);

            var validItem = _sampleData.NewItem();
            var invalidItem = _sampleData.NewItemWithInvalidKey();
            var items = new List<Activity>() { validItem, invalidItem };

            //Act
            bl.ReceiveActivityList( items );

            //Assert
            mockedDAL.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Once);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void RegisterNewItemWithNullKey()
        {
            //Arrange
            Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
            mockedDAL.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
            ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);

            var newItem = _sampleData.NewItem();
            newItem.Item.ItemRegistration.RegistrationKey = null;

            //Act
            bl.ReceiveActivityList(new List<Activity>() { newItem });

            //Assert
            mockedDAL.Verify(x => x.RegisterItem( It.IsAny<Activity>()), Times.Never());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ReadItem()
        {
            try
            {
                //Arrange
                Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
                mockedDAL.Setup(x => x.GetItemRegistration(It.IsAny<string>())).Returns(new ItemRegistration());
                mockedDAL.Setup(x => x.ReadItem(It.IsAny<Activity>())).Returns(true);

                //Act
                ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);
                bl.ReceiveActivityList(new List<Activity>() { _sampleData.RegisteredItemWithData("test") });

                //Assert
                mockedDAL.Verify(x => x.GetItemRegistration(It.IsAny<string>()), Times.Once);
                mockedDAL.Verify(x => x.ReadItem(It.IsAny<Activity>()), Times.Once);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ReadUnRegisteredItem()
        {
            try
            {
                Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
                mockedDAL.Setup(x => x.GetItemRegistration(It.IsAny<string>())).Returns((ItemRegistration) null);
                mockedDAL.Setup( x=> x.ReadItem( It.IsAny<Activity>() )).Returns(true);

                ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);
                bl.ReceiveActivityList(new List<Activity>() { _sampleData.UnRegisteredItemWithData() });

                //Assert
                mockedDAL.Verify(x => x.GetItemRegistration( It.IsAny<string>() ), Times.Once);
                mockedDAL.Verify(x => x.ReadItem(It.IsAny<Activity>()), Times.Never);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterAndReadItem()
        {
            try
            {
                //Arrange
                Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
                mockedDAL.Setup(x => x.RegisterReadItem(It.IsAny<Activity>())).Returns(1);
                mockedDAL.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
                mockedDAL.Setup(x => x.ReadItem(It.IsAny<Activity>())).Returns(true);

                //Act
                ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);
                bl.ReceiveActivityList(new List<Activity>() { _sampleData.NewItemWithData() });

                //Assert
                mockedDAL.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Never);
                mockedDAL.Verify(x => x.ReadItem(It.IsAny<Activity>()), Times.Never);
                mockedDAL.Verify(x => x.RegisterReadItem(It.IsAny<Activity>()), Times.Once);
                
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void NewItemsWithNullValues()
        {
            try
            {
                //Arrange
                Mock<IReceiveAdapterRepository> mockedDAL = new Mock<IReceiveAdapterRepository>();
                mockedDAL.Setup(x => x.RegisterReadItem(It.IsAny<Activity>())).Returns(1);
                mockedDAL.Setup(x => x.RegisterItem(It.IsAny<Activity>())).Returns(1);
                mockedDAL.Setup(x => x.ReadItem(It.IsAny<Activity>())).Returns(true);

                //Act
                ReceiveAdapterBL bl = new ReceiveAdapterBL(mockedDAL.Object, _registrationKey);
                bl.ReceiveActivityList(_sampleData.NewItemsWithNullValues());

                //Assert
                mockedDAL.Verify(x => x.RegisterItem(It.IsAny<Activity>()), Times.Never);
                mockedDAL.Verify(x => x.ReadItem(It.IsAny<Activity>()), Times.Never);
                mockedDAL.Verify(x => x.RegisterReadItem(It.IsAny<Activity>()), Times.Never);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }
    }
}
