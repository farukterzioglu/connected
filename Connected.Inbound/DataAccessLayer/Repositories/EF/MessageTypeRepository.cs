using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer.Repositories.EF
{
    class MessageTypeRepository : IMessageTypeRepositoty
    {
        public List<MessageType> GetAll()
        {
            using (var context = new ConnectedConfEntities())
            {
                return context.MessageType.ToList();
            }
        }
    }
}
