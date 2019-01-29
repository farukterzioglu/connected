using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.Configuration.Repositories
{
    public interface IMessageTypeRepositoty
    {
        List<MessageTypeDTO> GetAll();
    }
}
