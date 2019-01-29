using System.Collections.Generic;
using Connected.DAL;

namespace Schemas.SendAdapter
{
    public interface IDataGeter
    {
        List<Activity> GetData();
        List<Activity> GetRegistrationData();
    }
}
