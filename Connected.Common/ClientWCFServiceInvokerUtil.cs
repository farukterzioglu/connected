using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using Connected.Schemas.Common;

namespace Connected.Common
{
    /// <summary>
    /// This class has utility methods to invoke a service, given a URI and method name
    /// </summary>
    public class ClientWCFServiceInvokerUtil
    {
        #region Methods

        //TODO : ? Instead of caching maybe creating ChannelFactory as static 
        //public static ChannelFactory<IReceiveAdapter> recvAdapterChannelFactory;
        public static void CallReceiveAdapterWcfService(Uri uri, TransferMessage transferMessage)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(uri);

            //TODO : Cache ChannelFactory according to URL
            ChannelFactory<IReceiveAdapter> recvAdapterChannelFactory = 
                new ChannelFactory<IReceiveAdapter>(binding, endpoint);

            // Create a channel.
            var wcfClient = recvAdapterChannelFactory.CreateChannel();

            wcfClient.ProcessMessage(transferMessage);

            ((IClientChannel)wcfClient).Close();
            
            recvAdapterChannelFactory.Close();
        }

        /// <summary>
        /// This method will call a WCF Service that implements the IInboundService Contract
        /// </summary>
        /// <param name="activeEndPointName">End point name</param>
        /// <param name="transferMessage">The message that needs to be submitted</param>
        public static void CallReceiveAdapterWcfService(string activeEndPointName, TransferMessage transferMessage)
        {
            var recvAdapterChannelFactory = new ChannelFactory<IReceiveAdapter>(activeEndPointName);

            // Create a channel.
            var wcfClient = recvAdapterChannelFactory.CreateChannel();

            // send the message to the adapter
            wcfClient.ProcessMessage(transferMessage);
            ((IClientChannel)wcfClient).Close();
        }

        /// <summary>
        /// Create Receive Adapter Service Channel Factory
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ChannelFactory<IReceiveAdapter> CreateReceiveAdapterServiceChannelFactory(string url)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(url);

            ChannelFactory<IReceiveAdapter> recvAdapterChannelFactory =
                new ChannelFactory<IReceiveAdapter>(binding, endpoint);

            return recvAdapterChannelFactory;
        }

        /// <summary>
        /// Call Receive Adapter Service with existing Channel Factory
        /// </summary>
        /// <param name="channelFactory"></param>
        /// <param name="transferMessage"></param>
        public static void CallReceiveAdapterWcfService(ChannelFactory<IReceiveAdapter> channelFactory, TransferMessage transferMessage)
        {
            var wcfClient = channelFactory.CreateChannel();
            wcfClient.ProcessMessage(transferMessage);
            ((IClientChannel)wcfClient).Close();
        }


        //TODO : Cache or make static the channelFactory
        /// <summary>
        /// This method will call a WCF Service that implements the IInboundService Contract
        /// </summary>
        /// <param name="activeEndPointName">End point name</param>
        /// <param name="transferMessage">The message that needs to be submitted</param>
        /// <param name="adapterName"> the name of the adapter that is sending the message back into connect</param>
        public static TransferMessage CallInboundWcfService(string activeEndPointName, TransferMessage transferMessage, string adapterName)
        {
            var recvAdapterChannelFactory = new ChannelFactory<IInboundService>(activeEndPointName);

            // Create a channel.
            var wcfClient = recvAdapterChannelFactory.CreateChannel();

            // send the message to the adapter
            var response = wcfClient.ProcessTransferMessage(transferMessage, adapterName);
            ((IClientChannel)wcfClient).Close();

            return response;
        }

        private static TransferMessage CallInboundWcfService(string activeEndPointName, TransferMessage canonicalMessage, string adapterName, string userName, string password, string domain)
        {
            var recvAdapterChannelFactory = new ChannelFactory<IInboundService>(activeEndPointName);

            // step one - find and remove default endpoint behavior 
            var defaultCredentials = recvAdapterChannelFactory.Endpoint.Behaviors.Find<ClientCredentials>();
            recvAdapterChannelFactory.Endpoint.Behaviors.Remove(defaultCredentials);

            // step two - instantiate your credentials
            ClientCredentials loginCredentials = new ClientCredentials();

            loginCredentials.Windows.ClientCredential.UserName = userName;
            loginCredentials.Windows.ClientCredential.Password = password;

            //loginCredentials.UserName.UserName = userName;
            //loginCredentials.UserName.Password = password;
            if (!string.IsNullOrEmpty(domain)) loginCredentials.Windows.ClientCredential.Domain = domain;

            // step three - set that as new endpoint behavior on factory
            recvAdapterChannelFactory.Endpoint.Behaviors.Add(loginCredentials); //add required ones


            // Create a channel.
            var wcfClient = recvAdapterChannelFactory.CreateChannel();

            // send the message to the adapter
            var transferMessage = wcfClient.ProcessTransferMessage(canonicalMessage, adapterName);
            ((IClientChannel)wcfClient).Close();

            return transferMessage;
        }

        #endregion Methods
    }
}