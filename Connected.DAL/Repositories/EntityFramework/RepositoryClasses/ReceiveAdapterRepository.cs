using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Connected.DAL.Repositories.Interfaces;

// ReSharper disable once CheckNamespace
namespace Connected.DAL.Repositories.EntityFramework
{
    public class ReceiveAdapterRepository : IReceiveAdapterRepository
    {
        private readonly ConnectedEntities _context;

        public ReceiveAdapterRepository(ConnectedEntities context) 
        {
            if (context != null)
                this._context = context;
            else
                throw new ArgumentNullException("context");
        }

        public int RegisterItem(Activity activityInput)
        {
            string desciption = "";

            if (activityInput == null) throw new NullReferenceException();
            if (activityInput.Item == null 
                || activityInput.Item.ItemName == null 
                || activityInput.Item.OriginalId == null
                ) return -1;

            #region NOTE : Lazy loading, ObjectContext from DBContext, Projection Query
            ////NOTE : Lazy loading prevention example
            ////Eager loading ->
            //context.Configuration.LazyLoadingEnabled = false;
            //var items = context.Item.Include( x=> x.Activity).Where(x => x.Id == 3).ToList();
            //foreach (var oneItem in items)
            //{
            //    //If we don't use Include on above query, the below query will be make lazy loading to get Activities.
            //    //By adding Include, we prevent lazy loding. This will effect performance positivly
            //    foreach (var oneActivity in oneItem.Activity)
            //    {
            //        var adj = oneActivity.ActivityTypeId;
            //    }
            //}

            ////NOTE : Getting object context from DBContext, note for future use
            //var objectContext = (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

            ////NOTE : Projection Query
            //private class AltItemType
            //{
            //    public int Id;
            //    public string Name;
            //}
            //List<AltItemType> altItemTypeList = context.ItemType.Select(x => new AltItemType() {  Id = x.Id, Name = x.Name }).ToList();
            //var altItemTypeList1 = context.ItemType.Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            #endregion

            //Add or get activity type
            ActivityType actType;

            if (activityInput.ActivityType != null && activityInput.ActivityType.TypeName != null)
            {
                actType = _context.ActivityType.FirstOrDefault(x => x.TypeName == activityInput.ActivityType.TypeName) ??
                          _context.ActivityType.Add(new ActivityType() { TypeName = activityInput.ActivityType.TypeName });
            }
            else
            {
                actType = _context.ActivityType.Add(new ActivityType() { TypeName = "Undefined Type" });
            }

            //Add activity
            Activity activityDB = new Activity()
            {
                ActivityTypeId = actType.Id,
                DateTime = activityInput.DateTime
            };

            //Check if Item exist
            var itemDb = _context.Item.Include( x => x.Capacity).FirstOrDefault(x => x.OriginalId == activityInput.Item.OriginalId);

            //Add new item
            if (itemDb == null)
            {
                //Add or get item type
                ////NOTE : Get all types without change tracking 
                //List<ItemType> itemTypes = context.ItemType.AsNoTracking().ToList();
                ItemType itemType;
                if (activityInput.Item.ItemType != null && activityInput.Item.ItemType.Name != null)
                {
                    itemType = _context.ItemType.FirstOrDefault(x => x.Name == activityInput.Item.ItemType.Name) ??
                               _context.ItemType.Add(new ItemType() { Name = activityInput.Item.ItemType.Name });
                }
                else
                {
                    itemType = _context.ItemType.Add(new ItemType() { Name = "Undefined Type" });
                }

                //Set item
                Item item = new Item()
                {
                    ItemTypeId = itemType.Id,
                    ItemName = activityInput.Item.ItemName,
                    OriginalId = activityInput.Item.OriginalId
                };

                //Add capacity info
                if (activityInput.Item.Capacity != null)
                {
                    var capacity = _context.Capacity.Add(
                        new Capacity()
                        {
                            ItemOn = activityInput.Item.Capacity.ItemOn,
                            ItemOff = activityInput.Item.Capacity.ItemOff,
                            HasAdjustables = activityInput.Item.Capacity.HasAdjustables,
                            Readable = activityInput.Item.Capacity.Readable
                        });

                    item.CapacityId = capacity.Id;

                    desciption += " | Capacity added.";
                }

                //Add registration
                if (activityInput.Item.ItemRegistration != null)
                {
                    var registration = _context.ItemRegistration.Add(
                        new ItemRegistration()
                        {
                            RegistrationDate = activityInput.Item.ItemRegistration.RegistrationDate,
                            AccessURL = activityInput.Item.ItemRegistration.AccessURL,
                            isRestful = activityInput.Item.ItemRegistration.isRestful,
                            RegistrationKey = activityInput.Item.ItemRegistration.RegistrationKey
                        });

                    item.RegistrationId = registration.Id;

                    desciption += " | Registration added.";
                }

                //Add item
                item = _context.Item.Add(item);
                desciption += " | New Item added.";

                activityDB.ItemId = item.Id;
            }
            else //item != null ->
            {
                activityDB.ItemId = itemDb.Id;
                desciption += " | Item already exist. (" + itemDb.Id+ ")";

                //Add or update capacity
                if(activityInput.Item.Capacity != null)
                {
                    //Add capacity
                    if (itemDb.CapacityId == null)
                    {
                        var capacity = _context.Capacity.Add(
                            new Capacity()
                            {
                                ItemOn = activityInput.Item.Capacity.ItemOn,
                                ItemOff = activityInput.Item.Capacity.ItemOff,
                                HasAdjustables = activityInput.Item.Capacity.HasAdjustables,
                                Readable = activityInput.Item.Capacity.Readable
                            });

                        itemDb.CapacityId = capacity.Id;
                        _context.Entry(itemDb).State = EntityState.Modified;

                        desciption += " | Capacity added.";
                    }
                    //Update capacity
                    else if (itemDb.CapacityId != null)
                    {
                        itemDb.Capacity.ItemOn = activityInput.Item.Capacity.ItemOn;
                        itemDb.Capacity.ItemOff = activityInput.Item.Capacity.ItemOff;
                        itemDb.Capacity.HasAdjustables = activityInput.Item.Capacity.HasAdjustables;
                        itemDb.Capacity.Readable = activityInput.Item.Capacity.Readable;

                        _context.Entry(itemDb.Capacity).State = EntityState.Modified;

                        desciption += " | Capacity updated.";
                    }
                }                

                //Add registration
                if (itemDb.RegistrationId == null &&
                    activityInput.Item.ItemRegistration != null)
                {
                    var registration = _context.ItemRegistration.Add(
                        new ItemRegistration()
                        {
                            RegistrationDate = activityInput.Item.ItemRegistration.RegistrationDate,
                            AccessURL = activityInput.Item.ItemRegistration.AccessURL,
                            isRestful = activityInput.Item.ItemRegistration.isRestful,
                            RegistrationKey = activityInput.Item.ItemRegistration.RegistrationKey
                        });

                    itemDb.RegistrationId = registration.Id;
                    _context.Entry(itemDb).State = EntityState.Modified;

                    desciption += " | Registration added.";
                }
            }
            activityDB.Description = desciption;

            activityDB = _context.Activity.Add(activityDB);
            
            _context.SaveChanges();

            if (activityDB.Item.RegistrationId != null)
                return activityDB.Item.RegistrationId ?? -1;
            else
                return -1;
        }

        public bool ReadItem(Activity activityInput)
        {
            //TODO : Implement description
            string desc = "";

            if (activityInput == null) throw new NullReferenceException();
            if (activityInput.Item == null) return false;

            //Add or get activity type
            ActivityType actType = _context.ActivityType.FirstOrDefault(x => x.TypeName == activityInput.ActivityType.TypeName) ??
                                   _context.ActivityType.Add(new ActivityType() { TypeName = activityInput.ActivityType.TypeName });

            //Add activity
            Activity activityDb = new Activity()
            {
                ActivityTypeId = actType.Id,
                DateTime = activityInput.DateTime
            };

            //Check if Item exist
            Item itemDb = _context.Item.Where(x => x.OriginalId == activityInput.Item.OriginalId).Include( x=> x.Adjustable).FirstOrDefault();

            //If item is null, send to register
            if (itemDb == null)
            {
                RegisterItem(activityInput);
                itemDb = _context.Item.Where(x => x.OriginalId == activityInput.Item.OriginalId).Include(x => x.Adjustable).FirstOrDefault();
                if(itemDb != null)
                    desc += " | Item registered.";
            }

            if (itemDb == null)
                return false;
            else
                activityDb.ItemId = itemDb.Id;
            
            //Add or update adjustables
            if (activityInput.Item.Adjustable != null && activityInput.Item.Adjustable.Count > 0)
            {
                //Adjustable types list
                List<AdjustableType> adjTypeList = _context.AdjustableType.ToList();
                List<AdjustableType> tempAdjTypeList = new List<AdjustableType>();

                foreach (Adjustable adjustableInput in activityInput.Item.Adjustable)
                {
                    //Check if adjustables exist
                    Adjustable adjustableDb = itemDb.Adjustable.FirstOrDefault(
                        x =>    x.ItemId == itemDb.Id &&  
                                x.IdentifierName == adjustableInput.IdentifierName);

                    //Update
                    if (adjustableDb != null)
                    {
                        adjustableDb.Value = adjustableInput.Value;
                        adjustableDb.ModifiedDateTime = adjustableInput.ModifiedDateTime;

                        _context.Entry(adjustableDb).State = EntityState.Modified;

                        desc += " | Adjustable updated. (" + adjustableDb.Id + ")";
                    }
                    //Add
                    else
                    {
                        adjustableDb = new Adjustable()
                        {
                            ItemId = itemDb.Id,
                            Name = adjustableInput.Name,
                            Value = adjustableInput.Value,
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            IdentifierName = adjustableInput.IdentifierName
                        };

                        //Add or get adjustable type 
                        AdjustableType adjType = adjTypeList.FirstOrDefault(x => x.TypeName == adjustableInput.AdjustableType.TypeName);
                        AdjustableType tempAdjType = tempAdjTypeList.FirstOrDefault(x => x.TypeName == adjustableInput.AdjustableType.TypeName);
                                         
                        //DB has this type 
                        if(adjType != null)
                        {
                            adjustableDb.TypeId = adjType.Id;
                        }
                        //Temp list has this type 
                        else if(tempAdjType != null)
                        {
                            adjustableDb.AdjustableType = tempAdjType;
                        }
                        //Type doesn't exist. Add to db and to temp list
                        else //if (adjType == null && tempAdjType == null)
                        {
                            var type = new AdjustableType() { TypeName = adjustableInput.AdjustableType.TypeName };
                            adjustableDb.AdjustableType = type;
                            tempAdjTypeList.Add(type);

                            desc += " | Adjustable type added."; 
                        }

                        _context.Adjustable.Add(adjustableDb);
                        desc += " | Adjustable added. (" + adjustableDb.Id + ")";
                    }                    
                }
                //Add activity
                activityDb.Description = desc;
                activityDb = _context.Activity.Add(activityDb);

                _context.SaveChanges();

                return true;
            }
            else
            {
                
                return false;
            }
        }

        public int RegisterReadItem(Activity activityInput)
        {
            var regId = RegisterItem(activityInput);
            if (regId > 0)
                ReadItem(activityInput);

            return regId;
        }

        public ItemRegistration GetItemRegistration(string originalId)
        {
            if (originalId == null) 
                throw new ArgumentNullException("originalId");

            var item = _context.Item.Where(x => x.OriginalId == originalId).Include(x => x.ItemRegistration).FirstOrDefault();

            if (item != null)
                return item.ItemRegistration ?? null;
            else
                return null;
        }
    }
}
