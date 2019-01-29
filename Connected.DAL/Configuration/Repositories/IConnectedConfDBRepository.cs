using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.Configuration.Repositories
{
    public interface IConnectedConfDBRepository
    {
        List<MessageTypeDTO> GetMessageTypes();
        List<AdapterBasicDTO> GetAdapters(int? adapterType = null);
        List<AdapterBasicDTO> GetSendAdapters();
        List<AdapterBasicDTO> GetReceiveAdapters();
        List<AdapterBasicDTO> GetReceiveAdaptersWithDetails();
        List<AdapterMessageTypeDTO> GetAdapterMessageTypes();
        List<MessageSubscriptionDetailsDTO> GetMessageSubscriptionDetails();
    }
}
