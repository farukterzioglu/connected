using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums
    {
        public enum ActivityTypeEnum
        {
            RegisterItem = 1,
            ReadItem = 2,
            RegisterReadItem = 3,
            WriteItem = 4,
            CapacityUpdate = 5
        }

        public enum LogType
        {
            Info = 1,
            Warning = 2,
            Error  = 3
        }
    }
}
