using System.Collections.Generic;
using Connected.Schemas;
using Connected.Schemas.Common;

namespace Schemas.SendAdapter
{
    public interface ISendAdapter
    {
        IDataSender DataSender { get;}
        IDataGeter DataGeter { get;  }
        IReceiveAdapter DataReader { get; }
        bool Registered { get; set; }

        bool ReadData();
        bool Register();
        bool HostDataReader();
    }
}
