using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Connected.DAL.Configuration;
using Connected.DAL.Configuration.Repositories;
using Connected.DAL.Core.Configuration.Model;
using Connected.Schemas;
using Connected.Schemas.Common;
using Infrastructure.AspectOriented.Aspects;

namespace Connected.Inbound
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InboundService : IInboundService
    {
        public IMessageStorage MessageStorage { get; set; }
        public IConnectedConfDBRepository ConnectedConfDBRepository;

        readonly List<MessageTypeDTO> _messageTypes;
        readonly List<AdapterBasicDTO> _sendAdapters;
        readonly List<AdapterMessageTypeDTO> _registrations;

        public InboundService(IMessageStorage messageStorage, IConnectedConfDBRepository connectedConfDBRepository)
        {
            MessageStorage = messageStorage;
            ConnectedConfDBRepository = connectedConfDBRepository;

            _messageTypes = ConnectedConfDBRepository.GetMessageTypes();
            _sendAdapters = ConnectedConfDBRepository.GetSendAdapters();
            _registrations = ConnectedConfDBRepository.GetAdapterMessageTypes();

            //Decorator Pattern Test
            IMessageStorage loggedMessageStorage = new Connected.MessageStorage.LoggedMessagesDecorator(messageStorage);
        }

        [LogAspect]
        [TraceAspect]
        public TransferMessage ProcessTransferMessage(TransferMessage transferMessage, string adapterName)
        {
            //Check if send adapter registered or not 
            var sendAdapter = _sendAdapters.FirstOrDefault(x => x.AdapterName == adapterName);
            if (sendAdapter == null)
            {
                //TODO : Log unregistered message

                return new TransferMessage()
                {
                    Header = new TransferMessageHeader()
                    {
                        Destination = "Sender",
                        MessageType = "Send adapter didn'T registered.",
                        Source = "Command Center",
                        Status = "Rejected",
                        TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    },
                    Body = null 
                };
            }

            //TODO : Implement this 
            //Check message schema

            //Check if message type exist or not
            var transferMessageMessageType =
                _messageTypes.FirstOrDefault(x => x.MessageType1 == transferMessage.Header.MessageType);
            if (transferMessageMessageType == null)
            {
                //TODO : Log unregistered message type 

                return new TransferMessage()
                {
                    Header = new TransferMessageHeader()
                    {
                        Destination = "Sender",
                        MessageType = "message type  didn't registered.",
                        Source = "Command Center",
                        Status = "Rejected",
                        TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    },
                    Body = null
                };
            }

            //Add message to the queue
            try
            {
                MessageStorage.InsertMessage(new Message()
                {
                    Datetime = DateTime.Now,
                    MessageText = transferMessage.Body.OuterXml,
                    MessageType = transferMessageMessageType.Id
                });
            }
            catch (Exception ex)
            {
                //TODO : Log error
                throw ex;
            }

            //Return succesfull message
            return new TransferMessage()
            {
                Header = new TransferMessageHeader()
                {
                    Destination = "Sender",
                    MessageType = "Message Succesfully Received.",
                    Source = "Command Center",
                    Status = "Received",
                    TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                },
                Body = null
            };
        }
    }
}
