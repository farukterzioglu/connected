using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.Configuration.Repositories.EF
{
    class MessageTypeRepository : IMessageTypeRepositoty
    {
        public List<MessageTypeDTO> GetAll()
        {
            using (var context = new ConnectedConfEntities())
            {
                return context.MessageType.Select( x=> new MessageTypeDTO()
                {
                    MessageType1 = x.MessageType1,
                    Id =  x.MessageTypeId,
                    MessageSchema = x.MessageSchema,
                    ModifiedDate = x.ModifiedDate,
                    RegistrationDate = x.RegistrationDate
                }).ToList();
            }
        }
    }
}
