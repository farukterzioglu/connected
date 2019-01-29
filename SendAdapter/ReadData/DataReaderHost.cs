using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;


namespace SendAdapter.ReadData
{
    public class DataReaderHost
    {
        public void HostDataReader()
        {
            ServiceHost serviceHost = new ServiceHost(typeof(DataReader));
            serviceHost.Open();
        }
    }
}
