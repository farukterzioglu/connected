using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core.Configuration.Model;
using Connected.Infrastructure;

namespace Connected.DAL.Configuration.Repositories.EF
{
    public class AdaptersRepository : IAdaptersRepository
    {
        public List<AdapterBasicDTO> GetAllAdapters(int? adapterType = null)
        {
            throw new NotImplementedException("Mapping of DTO not completed.");
            using (var context = new ConnectedConfEntities())
            {
                var list = new List<AdapterBasic>();
                
                if(adapterType != null)
                    list = context.AdapterBasic.Where( x=> x.AdapterTypeId == adapterType).ToList();
                else
                    list = context.AdapterBasic.ToList();

                return list.Select(x => new AdapterBasicDTO() {}).ToList();
            }
        }
    }
}
