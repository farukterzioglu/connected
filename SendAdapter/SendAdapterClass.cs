using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL;
using Connected.Schemas;
using Connected.Schemas.Common;
using Schemas;
using Schemas.SendAdapter;
using SendAdapter.GetData;
using SendAdapter.ReadData;
using SendAdapter.SendData;

namespace SendAdapter
{
    public class SendAdapterClass : ISendAdapter
    {
        private IDataSender _dataSender;
        private IDataGeter _dataGeter;
        private IReceiveAdapter _dataReader;

        public bool Registered { get; set; }

        public SendAdapterClass()
        {
            Registered = Register();
        }

        public IDataSender DataSender
        {
            get { return _dataSender ?? (_dataSender = new DataSender()); }
        }

        public IDataGeter DataGeter
        {
            get { return _dataGeter ?? (_dataGeter = new DataGeter()); }
        }

        public IReceiveAdapter DataReader
        {
            get { return _dataReader ?? (_dataReader = new DataReader()); }
        }

        private bool SendActivityList(List<Activity> activityList)
        {
            if (!activityList.Any()) return false;

            try
            {
                try
                {
                    DataSender.SendData(activityList);
                    //TODO : Log //Console.WriteLine("Activity send to Receivce Adapter");
                    return true;
                }
                catch (Exception ex)
                {
                    DataSender.AddFailedMessagesToQueue(activityList);
                    //TODO : Log //Console.WriteLine("Message sending failed, send to queue");
                    return true;
                }
            }
            catch (Exception ex)
            {
                //TODO : Log error
                throw ex;
                return false;
            }
        }

        public bool Register()
        {
            List<Activity> activityList = DataGeter.GetRegistrationData();
            return SendActivityList(activityList);
        }

        public bool ReadData()
        {
            List<Activity> activityList = DataGeter.GetData();
            return SendActivityList(activityList);
        }

        public bool HostDataReader()
        {
            throw new NotImplementedException();
        }
    }
}
