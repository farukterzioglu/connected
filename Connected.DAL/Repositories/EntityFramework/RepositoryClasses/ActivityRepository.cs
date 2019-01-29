using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.Interfaces;

namespace Connected.DAL.Repositories.EntityFramework
{
    public class ActivityRepository : AbstractRepository<Activity>, IActivityRepository
    {
        public List<Activity> getActivitiesByItemId(int itemId)
        {
            //return FindAll().Where(x => x.ItemId == itemId).ToList();
            return FindAll( x=> x.ItemId == itemId).ToList();
        }
    }
}
