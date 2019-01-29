using System.Collections.Generic;
using Connected.DAL;

namespace Schemas.SendAdapter
{
    public interface IDataSender
    {
        void SendData(List<Activity> activityList);
        void AddFailedMessagesToQueue(List<Activity> activityList);
    }
}
