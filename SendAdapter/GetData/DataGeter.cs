using System;
using System.Collections.Generic;
using System.Linq;
using SendAdapter.GetData.EkinCounter;

using System.Data.Entity;
using Connected.DAL;

namespace SendAdapter.GetData
{
    //TODO Singleton olmalı
    public class DataGeter : Schemas.SendAdapter.IDataGeter
    {
        public List<KeyValuePair<string, int>> HistoricalList;
        public void DateGeter()
        {
            HistoricalList = new List<KeyValuePair<string, int>>();

            //Get counters and latest historicals
            using (EkinCounterEntities context = new EkinCounterEntities())
            {
                var counters = context.Counters.AsNoTracking().ToList();
                var counterHistorical =
                    context.CounterHistorical.Where(historical => counters.Select(counter => counter.Id).ToList().Contains(historical.CounterId))
                        .OrderByDescending( x=> x.Id)
                        .Take( counters.Count())
                        .Include( x=> x.Counters)
                        .ToList();

                HistoricalList = counterHistorical.Select(x => new KeyValuePair<string, int>(x.Counters.UniqueId, x.Id)).ToList();
            }
        }

        private List<Item> ReadValues()
        {
            using (EkinCounterEntities context = new EkinCounterEntities())
            {
                var counters = context.Counters.AsNoTracking().ToList();

                List<CounterHistorical> counterHistoricals = 
                    counters.
                    Select(counter => context.CounterHistorical.Where(historical => historical.CounterId == counter.Id).
                        OrderByDescending(x => x.Id).
                        Include(x => x.Counters).
                        FirstOrDefault()).ToList();

                List<Item> counterItems = new List<Item>();

                foreach (var historical in counterHistoricals)
                {
                    //Check key-value pair list 

                    var item = new Item()
                    {
                        ItemType = new ItemType()
                        {
                            Name = historical.Counters.Name
                        },
                        ItemName = historical.Counters.Name,
                        Capacity = new Capacity()
                        {
                            HasAdjustables = true,
                            Readable = true,
                            ItemOn = true,
                            ItemOff = true
                        },
                        ItemRegistration = new ItemRegistration()
                        {
                            //TODO : Need to update counter table
                            AccessURL = "", //Add to DB
                            isRestful = false,
                            RegistrationKey = "123" //Add to DB
                        },
                        OriginalId = historical.Counters.UniqueId,
                        Adjustable = new List<Adjustable>()
                        {
                            new Adjustable()
                            {
                                AdjustableType = new AdjustableType() {TypeName = "Counter Value"},
                                Name = "Value",
                                Value = historical.Counters.Value.ToString(),
                                ModifiedDateTime = historical.Counters.ModifiedDateTime,
                                IdentifierName = historical.Counters.UniqueId
                            }
                        }
                    };
                    
                    counterItems.Add(item);
                }
                //var counters = context.Counters.AsNoTracking().ToList();

                //List<Item> counterItems = counters.Select(counter => new Item()
                //{
                //    ItemType = new ItemType()
                //    {
                //        Name = counter.Name
                //    },
                //    ItemName = counter.Name,
                //    Capacity = new Capacity()
                //    {
                //        HasAdjustables = true, Readable = true, ItemOn = true, ItemOff = true
                //    },
                //    ItemRegistration = new ItemRegistration()
                //    {
                //        //TODO : Need to update counter table
                //        AccessURL = "", //Add to DB
                //        isRestful = false,
                //        RegistrationKey = "123" //Add to DB
                //    },
                //    OriginalId = counter.UniqueId,
                //    Adjustable = new List<Adjustable>()
                //    {
                //        new Adjustable()
                //        {
                //            AdjustableType = new AdjustableType(){ TypeName = "Counter Value"},
                //            Name = "Value",
                //            Value = counter.Value.ToString(),
                //            ModifiedDateTime = counter.ModifiedDateTime,
                //            IdentifierName = counter.UniqueId
                //        }
                //    }
                //}).ToList();

                return counterItems;
            }
        }
        private List<Item> ReadRegistrations()
        {
            using (EkinCounterEntities context = new EkinCounterEntities())
            {
                var counters = context.Counters.AsNoTracking().ToList();

                List<Item> counterItems = counters.Select(counter => new Item()
                {
                    ItemType = new ItemType()
                    {
                        Name = "Counter"
                    },
                    ItemName = counter.Name,
                    Capacity = new Capacity()
                    {
                        HasAdjustables = true,
                        Readable = true,
                        ItemOn = true,
                        ItemOff = true
                    },
                    ItemRegistration = new ItemRegistration()
                    {
                        //TODO : Need to update counter table
                        AccessURL = "", //Add to Counter DB
                        isRestful = false,
                        RegistrationKey = "123", //Add to Counter DB
                        RegistrationDate = DateTime.Now
                    },
                    OriginalId = counter.UniqueId
                }).ToList();

                return counterItems;
            }
        }
        
        public List<Activity> GetData()
        {
            //Get Item Info
            var items = ReadValues();

            List<Activity> activityList = new List<Activity>();

            foreach (var item in items)
            {
                //Create Activity
                activityList.Add(new Activity()
                {
                    ActivityType = new ActivityType()
                    {
                        TypeName = Common.Enums.ActivityTypeEnum.ReadItem.ToString()
                    },
                    Item = item,
                    DateTime = DateTime.Now,
                    Description = "Data reading from Counter; " + item.ItemName
                });    
            }

            return activityList;
        }
        public List<Activity> GetRegistrationData()
        {
            //Get Item Info
            var items = ReadRegistrations();

            List<Activity> activityList = new List<Activity>();

            foreach (var item in items)
            {
                //Create Activity
                Activity activity = new Activity()
                {
                    ActivityType = new ActivityType()
                    {
                        TypeName = Common.Enums.ActivityTypeEnum.RegisterItem.ToString()
                    },
                    Item = item,
                    DateTime = DateTime.Now,
                    Description = "Data reading from Counter; " + item.ItemName
                };

                activityList.Add(activity);
            }

            return activityList;
        }
    }
}
