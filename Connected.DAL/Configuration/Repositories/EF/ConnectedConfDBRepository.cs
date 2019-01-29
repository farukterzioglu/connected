using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.Configuration.Repositories.EF
{
    public class ConnectedConfDBRepository : IConnectedConfDBRepository
    {
        public List<MessageTypeDTO> GetMessageTypes()
        {
            return new MessageTypeRepository().GetAll();
        }

        public List<AdapterBasicDTO> GetAdapters(int? adapterType = null)
        {
            return new AdaptersRepository().GetAllAdapters(adapterType);
        }

        //TODO : Use repositories
        public List<AdapterBasicDTO> GetSendAdapters()
        {
            throw new NotImplementedException("Mapping of DTO not completed.");

            using (var context = new ConnectedConfEntities())
            {
                var sendAdapterType = context.AdapterTypeDIM.FirstOrDefault(x => x.AdapterType == "SendAdapter");
                if (sendAdapterType == null) return null;

                var sendADapters =
                    context.AdapterBasic
                        .Where(x => x.AdapterTypeId == sendAdapterType.AdapterTypeId)
                        .Select( x=> new AdapterBasicDTO(){ })
                        .ToList();

                return sendADapters;
            }
        }

        //TODO : Use repositories
        public List<AdapterBasicDTO> GetReceiveAdapters()
        {
            throw new NotImplementedException("Mapping of DTO not completed.");

            using (var context = new ConnectedConfEntities())
            {
                var receiveAdapterType = context.AdapterTypeDIM.FirstOrDefault(x => x.AdapterType == "ReceiveAdapter");
                if (receiveAdapterType == null) return null;

                var receiveAdapters =
                    context.AdapterBasic
                        .Where(x => x.AdapterTypeId == receiveAdapterType.AdapterTypeId)
                        .Select( x=> new AdapterBasicDTO(){})
                        .ToList();

                return receiveAdapters;
            }
        }

        //TODO : Use repositories
        public List<AdapterBasicDTO> GetReceiveAdaptersWithDetails()
        {
            throw new NotImplementedException("Mapping of DTO not completed.");

            using (var context = new ConnectedConfEntities())
            {
                var receiveAdapterType = context.AdapterTypeDIM.FirstOrDefault(x => x.AdapterType == "ReceiveAdapter");
                if (receiveAdapterType == null) return null;

                var receiveAdapters =
                    context.AdapterBasic
                        .Where(x => x.AdapterTypeId == receiveAdapterType.AdapterTypeId)
                        .Include(x => x.ReceiveAdapterDetails)
                        .Select( x=> new AdapterBasicDTO(){})
                        .ToList();

                return receiveAdapters;
            }
        }

        //TODO : Use repositories
        public List<AdapterMessageTypeDTO> GetAdapterMessageTypes()
        {
            using (var context = new ConnectedConfEntities())
            {
                return context.AdapterMessageType.Select( x => new AdapterMessageTypeDTO(){}).ToList();
            }
        }

        public List<MessageSubscriptionDetailsDTO> GetMessageSubscriptionDetails()
        {
            using (var context = new ConnectedConfEntities())
            {
                return context.MessageSubscriptionDetails.Select( x=> new MessageSubscriptionDetailsDTO(){}).ToList();
            }
        }
    }
}
