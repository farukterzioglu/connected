using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.Interfaces;

namespace Connected.DAL.Repositories.ADONET
{
    public class ActivityRepository : IActivityRepository
    {
        public List<Activity> getActivitiesByItemId(int itemId)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Activity> FindAll(System.Linq.Expressions.Expression<Func<Activity, bool>> predicate, params System.Linq.Expressions.Expression<Func<Activity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Activity> FindAll(params System.Linq.Expressions.Expression<Func<Activity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
