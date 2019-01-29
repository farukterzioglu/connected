using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Infrastructure;

namespace Connected.DAL.Repositories.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        List<Activity> getActivitiesByItemId(int itemId);
    }
}
