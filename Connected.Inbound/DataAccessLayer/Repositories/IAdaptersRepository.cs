using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories
{
    public interface IAdaptersRepository
    {
        List<AdapterBasic> GetAllAdapters(int? adapterType = null);
    }
}
