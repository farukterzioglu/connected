using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories.EF
{
    public class ConnectedConfDBRepository : IConnectedConfDBRepository
    {
        public List<MessageType> GetMessageTypes()
        {
            return new MessageTypeRepository().GetAll();
        }

        public List<AdapterBasic> GetAdapters(int? adapterType = null)
        {
            return new AdaptersRepository().GetAllAdapters(adapterType);
        }

        public List<AdapterMessageType> GetAdapterMessageTypes()
        {
            throw new NotImplementedException();
        }
    }
}
