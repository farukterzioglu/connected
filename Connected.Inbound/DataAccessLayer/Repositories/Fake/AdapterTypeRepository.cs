using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories.Fake
{
    public class AdapterTypeRepository : IAdapterTypes
    {
        public List<AdapterTypeDIM> GetAdapterTypes(int? typeId = null)
        {
            var adapterTypes =  new List<AdapterTypeDIM>()
            {
                new AdapterTypeDIM()
                {
                    AdapterTypeId = 1,
                    AdapterType = "Send Adapter",
                    CreationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new AdapterTypeDIM()
                {
                    AdapterTypeId = 2,
                    AdapterType = "Receive Adapter",
                    CreationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                }
            };

            if (typeId != null)
                return adapterTypes.Where(x => x.AdapterTypeId == typeId).ToList();
            else
                return adapterTypes;
        }
    }
}
