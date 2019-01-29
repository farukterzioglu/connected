using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Connected.Common.SerializationHelper;
using Connected.DAL;
using Schemas.SendAdapter;
using Connected.Schemas;
using Connected.Schemas.Common;

namespace SendAdapter.SendData
{
    public class DataSender : IDataSender
    {
        //TODO : Remove Receive Adapter
        //private readonly SendAdapter.ReceiveAdapterServiceReference.ReceiveAdapterClient _client;

        public DataSender()
        {
            ////TODO : change this to Router Service
            //bool isDebug = true;

            //if (isDebug)
            //{
            //    _client = new SendAdapter.ReceiveAdapterServiceReference.ReceiveAdapterClient("BasicHttpBinding_IReceiveAdapter_Debug");
            //}
            //else
            //{
            //    _client = new SendAdapter.ReceiveAdapterServiceReference.ReceiveAdapterClient("BasicHttpBinding_IReceiveAdapter");
            //}

        }

        public void SendData(List<Activity> activityList)
        {
            var newTransferMesage = new TransferMessage()
            {
                Header = new TransferMessageHeader()
                {
                    Destination = "Inbound Service",
                    MessageType = "Counter Data",
                    Source = "Counter Data",
                    Status = "New Counter Data",
                    TimeStamp = DateTime.Now.ToString()
                },
                Body = SerializationHelper.SerializeObjectToXml<List<Activity>>(activityList).DocumentElement
            };

            //TODO : Check response, if not succesfull, log the message, or implement something like queue
            Connected.Common.ClientWCFServiceInvokerUtil.CallInboundWcfService("InBoundHttpEndPoint", newTransferMesage, "Counter Send Adapter");
        }

        public void AddFailedMessagesToQueue(List<Activity> activityList)
        {
            throw new NotImplementedException();
        }
    }
}
