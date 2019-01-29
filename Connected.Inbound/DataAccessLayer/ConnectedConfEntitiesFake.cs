using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Inbound.DataAccessLayer
{
    public class ConnectedConfEntitiesFake : IConnectedConfEntities
    {
        public ConnectedConfEntitiesFake()
        {
            throw new NotImplementedException("Don't use this one. Kept only for historical reasons");
        }

        public IDbSet<AdapterBasic> AdapterBasic { get; private set; }
        public IDbSet<AdapterMessageType> AdapterMessageType { get; private set; }
        public IDbSet<AdapterTypeDIM> AdapterTypeDIM { get; private set; }
        public IDbSet<ConnectedSettings> ConnectedSettings { get; set; }
        public IDbSet<MessageSubscriptionDetails> MessageSubscriptionDetails { get; private set; }
        public IDbSet<MessageType> MessageType { get; private set; }
        public IDbSet<ReceiveAdapterDetails> ReceiveAdapterDetails { get; private set; }
        public IDbSet<AdaptersView> AdaptersView { get; private set; }
        public IDbSet<ConnectedSettingsView> ConnectedSettingsView { get; private set; }

        public int SaveChanges()
        {
            return 1;
        }
    }
}
