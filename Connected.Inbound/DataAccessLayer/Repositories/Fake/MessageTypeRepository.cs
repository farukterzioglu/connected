using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories.Fake
{
    class MessageTypeRepository : IMessageTypeRepositoty
    {
        public List<MessageType> GetAll()
        {
            return new List<MessageType>()
            {
                new MessageType(){ MessageType1 = "Fake Message Type"}
            };
        }
    }
}
