using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.EntityFramework;
using Connected.DAL.Repositories.Interfaces;

namespace Connected.DAL.Repositories
{
    public class SampleActivityRepository : IActivityRepository
    {
        public IEnumerable<Activity> FindAll(Expression<Func<Activity, bool>> predicate, params Expression<Func<Activity, object>>[] includeProperties)
        {
            return new List<Activity>()
            {
                new Activity()
                {
                    ActivityTypeId = 1,
                    DateTime = DateTime.Now,
                    Description = "Description",
                    Id= 1,
                    Item  = new Item()
                    {
                        ItemType = new ItemType()
                        {
                            Id = 1,
                            Name = "Sample Type"
                        },
                        ItemName = "ItemName",
                        OriginalId = "123"
                    }
                }
            };   
        }

        public IQueryable<Activity> FindAll(params Expression<Func<Activity, object>>[] includeProperties)
        {
            return new List<Activity>()
            {
                new Activity()
                {
                    ActivityTypeId = 1,
                    DateTime = DateTime.Now,
                    Description = "Description",
                    Id= 1,
                    Item  = new Item()
                    {
                        ItemType = new ItemType()
                        {
                            Id = 1,
                            Name = "Sample Type"
                        },
                        ItemName = "ItemName",
                        OriginalId = "123"
                    }
                }
            }.AsQueryable();   
        }

        public Activity Add(Activity entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Activity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Activity entity)
        {
            throw new NotImplementedException();
        }

        public List<Activity> getActivitiesByItemId(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
