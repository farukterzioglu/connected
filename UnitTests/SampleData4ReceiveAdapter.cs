using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Common;
using Connected.DAL;

namespace UnitTests
{
    public class SampleData4ReceiveAdapter
    {
        private string registrationKey;
        public SampleData4ReceiveAdapter() 
        {
            //Read registraion key
            if (System.Configuration.ConfigurationManager.AppSettings["registrationKey"] != null)
            {
                registrationKey = System.Configuration.ConfigurationManager.AppSettings["registrationKey"];
            }
            else
                registrationKey = null;
        }

        public Activity NewItem()
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType(){ TypeName = "RegisterItem"},
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() 
                    { 
                        HasAdjustables = true, ItemOff = true, ItemOn = true, Readable = true 
                    },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType() 
                    { 
                        Name = "Test Item" 
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            };
            return Activity;
        }

        public List<Activity> MultipleNewItem()
        {
            List<Activity> list = new List<Activity>();

            list.Add( new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        ItemOff = true,
                        ItemOn = true,
                        Readable = true
                    },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType()
                    {
                        Name = "Test Item"
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            });

            list.Add(new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        ItemOff = true,
                        ItemOn = true,
                        Readable = true
                    },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType()
                    {
                        Name = "Test Item"
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            });

            list.Add(new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        ItemOff = true,
                        ItemOn = true,
                        Readable = true
                    },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType()
                    {
                        Name = "Test Item"
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            });

            list.Add(new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        ItemOff = true,
                        ItemOn = true,
                        Readable = true
                    },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType()
                    {
                        Name = "Test Item"
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            });

            return list;
        }

        public Activity NewItemWORegistrationInfo()
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType(){ TypeName = "RegisterItem"},
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        ItemOff = true,
                        ItemOn = true,
                        Readable = true
                    },
                    ItemName = "New Test Item",
                    ItemType = new ItemType()
                    {
                        Name = "Test Item"
                    },
                    OriginalId = Guid.NewGuid().ToString()
                }
            };
            return Activity;
        }

        public Activity NewItemWithInvalidKey()
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() { HasAdjustables = true, ItemOff = true, ItemOn = true, Readable = true },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = "InvalidKey"
                    },
                    ItemType = new ItemType() { Name = "Test Item" },
                    OriginalId = Guid.NewGuid().ToString()
                }
            };

            return Activity;
        }

        public List<Activity> NewItemsWithNullValues()
        {
            List<Activity> liste = new List<Activity>();

            liste.Add(new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() 
                    {
                        HasAdjustables = null,
                        ItemOff = null,
                        ItemOn = null,
                        Readable = null
                    },
                    ItemName = null,
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = null,
                        isRestful = null,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = null
                    },
                    ItemType = new ItemType()
                    {
                        Name = null
                    },
                    OriginalId = null,
                }
            });

            liste.Add(new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = null,
                    ItemName = null,
                    ItemRegistration = null,
                    ItemType = null,
                    OriginalId = null,
                }
            });

            liste.Add(new Activity()
            {
                ActivityType = null,
                DateTime = DateTime.Now,
                Item = null
            });


            return liste;
        }

        public Activity NewItemWithData()
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType() { TypeName = "RegisterReadItem" },
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() { HasAdjustables = true, ItemOff = true, ItemOn = true, Readable = true },
                    ItemName = "New Test Item",
                    ItemRegistration = new ItemRegistration()
                    {
                        AccessURL = "url:test.com",
                        isRestful = false,
                        RegistrationDate = DateTime.Now,
                        RegistrationKey = registrationKey
                    },
                    ItemType = new ItemType() { Name = "Test Item" },
                    OriginalId = Guid.NewGuid().ToString(),
                    Adjustable = new List<Adjustable>() 
                    {
                        #region Adjustables
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_OnOff",
                            AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Item On/Off",
                            Value = "On"
                        },
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_Valve1",
                            AdjustableType = new AdjustableType(){ TypeName = "Valve"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Valve 1",
                            Value = "150"
                        },
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_DoorOpen",
                            AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Door Open",
                            Value = "On"
                        }
                        #endregion                        
                    }

                }
            };
            return Activity;
        }

        public Activity RegisteredItemWithData(string itemOriginalId)
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType(){ TypeName = "ReadItem"},
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() { HasAdjustables = true, ItemOff = true, ItemOn = true, Readable = true },
                    ItemName = "New Updated Test Item",
                    OriginalId = itemOriginalId,
                    ItemType = new ItemType() { Name = "Test Item" },
                    Adjustable = new List<Adjustable>()
                    {
                        #region Adjustables
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_OnOff",
                            AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Item On/Off",
                            Value = "On"
                        }
                        //,
                        //new Adjustable()
                        //{
                        //    IdentifierName = "TestItem_Valve1",
                        //    AdjustableType = new AdjustableType(){ TypeName = "Valve"},
                        //    CreatedDateTime = DateTime.Now,
                        //    ModifiedDateTime = DateTime.Now,
                        //    Name = "Valve 1",
                        //    Value = "350"
                        //},
                        //new Adjustable()
                        //{
                        //    IdentifierName = "TestItem_DoorOpen",
                        //    AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                        //    CreatedDateTime = DateTime.Now,
                        //    ModifiedDateTime = DateTime.Now,
                        //    Name = "Door Open",
                        //    Value = "Off"
                        //}
                        #endregion
                    }
                }
            };
            return Activity;
        }

        public Activity UnRegisteredItemWithData()
        {
            var Activity = new Activity()
            {
                ActivityType = new ActivityType(){ TypeName = "ReadItem"},
                DateTime = DateTime.Now,
                Item = new Item()
                {
                    Capacity = new Capacity() { HasAdjustables = true, ItemOff = true, ItemOn = true, Readable = true },
                    ItemName = "New Updated Test Item",
                    OriginalId = Guid.NewGuid().ToString(),
                    ItemType = new ItemType() { Name = "Test Item" },

                    Adjustable = new List<Adjustable>() 
                    {
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_OnOff",
                            AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Item On/Off",
                            Value = "On"
                        },
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_Valve1",
                            AdjustableType = new AdjustableType(){ TypeName = "Valve"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Valve 1",
                            Value = "350"
                        },
                        new Adjustable()
                        {
                            IdentifierName = "TestItem_DoorOpen",
                            AdjustableType = new AdjustableType(){ TypeName = "OnOff"},
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            Name = "Door Open",
                            Value = "Off"
                        }
                    }
                }
            };

            return Activity;
        }
    }
}
