using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Infrastructure;

namespace Connected.Inbound.DataAccessLayer.Repositories.EF
{
    public class AdaptersRepository : IAdaptersRepository
    {
        public List<AdapterBasic> GetAllAdapters(int? adapterType = null)
        {
            using (var context = new ConnectedConfEntities())
            {
                if(adapterType != null)
                    return context.AdapterBasic.Where( x=> x.AdapterTypeId == adapterType).ToList();
                else
                    return context.AdapterBasic.ToList();
            }
        }
    }
}
