using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Connected.DAL;
using Connected.DAL.Repositories.EntityFramework;
using Connected.DAL.Repositories.Interfaces;


namespace UnitTests
{
    [TestClass]
    public class ReceiveAdapterDALEFTests
    {
        private readonly IReceiveAdapterRepository _receiveAdapterRepository;
        private readonly SampleData4ReceiveAdapter _sampleData;

        public ReceiveAdapterDALEFTests()
        {
            _sampleData = new SampleData4ReceiveAdapter();

            //Get&Set data context
            var dataContext = DataContextFactory.GetDataContext();

            //Set Data Access Layer
            _receiveAdapterRepository = new ReceiveAdapterRepository(dataContext);
        }

        [TestMethod]
        public void RegisterNewItem()
        {
            try
            {
                _receiveAdapterRepository.RegisterItem(_sampleData.NewItem());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterNewItemWithoutRegistration()
        {
            try
            {
                var activity = _sampleData.NewItem();
                activity.Item.ItemRegistration = null;

                _receiveAdapterRepository.RegisterItem(activity);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterExistingItemHasNoRegistration()
        {
            try
            {
                var activity = _sampleData.NewItem();
                var registrationInfo = new ItemRegistration()
                {
                    AccessURL = activity.Item.ItemRegistration.AccessURL,
                    isRestful = activity.Item.ItemRegistration.isRestful,
                    RegistrationDate = activity.Item.ItemRegistration.RegistrationDate,
                    RegistrationKey = activity.Item.ItemRegistration.RegistrationKey
                };

                activity.Item.ItemRegistration = null;
                _receiveAdapterRepository.RegisterItem(activity);

                activity.Item.ItemRegistration = registrationInfo;
                _receiveAdapterRepository.RegisterItem(activity);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ReRegisterItemUpdateCapacity()
        {
            try
            {
                var newItem = _sampleData.NewItem();
                _receiveAdapterRepository.RegisterItem(newItem);
                
                //Update capacity
                newItem.Item.Capacity.ItemOn = !newItem.Item.Capacity.ItemOn;
                newItem.Item.Capacity.ItemOff = !newItem.Item.Capacity.ItemOff;
                //Re-register
                _receiveAdapterRepository.RegisterItem(newItem);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterNewItemsWithNullValues()
        {
            try
            {
                var list = _sampleData.NewItemsWithNullValues();
                foreach (var item in list)
                {
                    int returnValue = _receiveAdapterRepository.RegisterItem(item);

                    Assert.IsTrue(returnValue == -1);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ReadRegisteredItem()
        {
            try
            {   
                var sampleActivity = _sampleData.NewItemWithData();

                //Register
                sampleActivity.ActivityType = new ActivityType() { TypeName = "RegisterItem" };
                _receiveAdapterRepository.RegisterItem(sampleActivity);

                //Read registered activity
                sampleActivity.ActivityType = new ActivityType() { TypeName = "ReadItem" };
                _receiveAdapterRepository.ReadItem(sampleActivity);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void UpdateRegisteredItem()
        {
            try
            {
                var sampleActivity = _sampleData.NewItemWithData();

                //Register
                sampleActivity.ActivityType = new ActivityType() { TypeName = "RegisterItem" };
                _receiveAdapterRepository.RegisterItem(sampleActivity);

                //Read registered activity
                sampleActivity.ActivityType = new ActivityType() { TypeName = "ReadItem" };
                _receiveAdapterRepository.ReadItem(sampleActivity);

                //Update
                var testItemOnOff = ((List<Adjustable>)sampleActivity.Item.Adjustable).FirstOrDefault(x => x.IdentifierName == "TestItem_OnOff");
                if (testItemOnOff != null)
                    testItemOnOff.Value = "Off";
                var testItemValve1 = ((List<Adjustable>)sampleActivity.Item.Adjustable).FirstOrDefault(x => x.IdentifierName == "TestItem_Valve1");
                if (testItemValve1 != null)
                    testItemValve1.Value = "50";
                var testItemDoorOpen = ((List<Adjustable>)sampleActivity.Item.Adjustable).FirstOrDefault(x => x.IdentifierName == "TestItem_DoorOpen");
                if (testItemDoorOpen != null)
                    testItemDoorOpen.Value = "Off";

                sampleActivity.ActivityType = new ActivityType() { TypeName = "ReadItem" };
                _receiveAdapterRepository.ReadItem(sampleActivity);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void GetItemRegistration()
        {
            try
            {   
                var sampleActivity = _sampleData.NewItem();

                //Register
                _receiveAdapterRepository.RegisterItem(sampleActivity);

                //Get registration info
                var registrationInfo = _receiveAdapterRepository.GetItemRegistration(sampleActivity.Item.OriginalId);

                Assert.IsNotNull(registrationInfo);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void RegisterReadNewItem()
        {
            try
            {
                _receiveAdapterRepository.RegisterReadItem(_sampleData.NewItemWithData());
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }

        }
    }
}