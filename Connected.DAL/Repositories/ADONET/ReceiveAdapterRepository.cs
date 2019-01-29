using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.Interfaces;

namespace Connected.DAL.Repositories.ADONET
{
    public class ReceiveAdapterRepository : IReceiveAdapterRepository
    {
        public int RegisterItem(Activity activity)
        {
            throw new NotImplementedException();
        }

        public bool ReadItem(Activity activity)
        {
            throw new NotImplementedException();
        }

        public int RegisterReadItem(Activity activity)
        {
            throw new NotImplementedException();
        }

        public ItemRegistration GetItemRegistration(string originalId)
        {
            throw new NotImplementedException();
        }
    }
}
