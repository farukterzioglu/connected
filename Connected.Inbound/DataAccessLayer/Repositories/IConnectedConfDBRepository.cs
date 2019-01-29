using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories
{
    public interface IConnectedConfDBRepository
    {
        List<MessageType> GetMessageTypes();
        List<AdapterBasic> GetAdapters(int? adapterType = null);
        List<AdapterMessageType> GetAdapterMessageTypes();
    }
}
