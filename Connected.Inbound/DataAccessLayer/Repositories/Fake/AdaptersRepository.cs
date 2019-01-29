using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories.Fake
{
    public class AdaptersRepository : IAdaptersRepository
    {
        public List<AdapterBasic> GetAllAdapters(int? adapterType = null)
        {
            var adapterTypes = new AdapterTypeRepository().GetAdapterTypes();

            var adapters =  new List<AdapterBasic>()
            {
                new AdapterBasic()
                {
                    AdapterId = 1,
                    AdapterName = "Mocked Send Adapter",
                    // ReSharper disable once PossibleNullReferenceException
                    AdapterTypeId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Send Adapter").AdapterTypeId,
                    RegistrationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                },
                new AdapterBasic()
                {
                    AdapterId = 2,
                    AdapterName = "Mocked Receive Adapter",
                    // ReSharper disable once PossibleNullReferenceException
                    AdapterTypeId = adapterTypes.FirstOrDefault(x => x.AdapterType == "Receive Adapter").AdapterTypeId,
                    RegistrationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                }
            };

            return adapterType == null ? 
                adapters : 
                adapters.Where(x => x.AdapterTypeId == adapterType).ToList();
        }

    }
}
