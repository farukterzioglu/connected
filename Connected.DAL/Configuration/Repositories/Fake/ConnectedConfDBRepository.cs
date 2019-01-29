﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.DAL.Configuration.Repositories.Fake
{
    class ConnectedConfDBRepository :IConnectedConfDBRepository
    {
        public List<MessageType> GetMessageTypes()
        {
            return new List<MessageType>()
            {
                new MessageType()
                {
                    MessageType1 = "Fake Message Type",
                    MessageSchema = "<FakeMessage></FakeMessage>",
                    ModifiedDate = DateTime.Now
                }
            };
        }

        public List<AdapterBasic> GetAdapters(int? adapterType = null)
        {
            throw new NotImplementedException();
        }

        public List<AdapterBasic> GetSendAdapters()
        {
            throw new NotImplementedException();
        }

        public List<AdapterBasic> GetReceiveAdapters()
        {
            throw new NotImplementedException();
        }

        public List<AdapterBasic> GetReceiveAdaptersWithDetails()
        {
            throw new NotImplementedException();
        }

        public List<AdapterMessageType> GetAdapterMessageTypes()
        {
            throw new NotImplementedException();
        }

        public List<MessageSubscriptionDetails> GetMessageSubscriptionDetails()
        {
            throw new NotImplementedException();
        }
    }
}
