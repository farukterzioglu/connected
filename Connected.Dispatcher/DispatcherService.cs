using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Connected.Common.SerializationHelper;
using Connected.DAL.Configuration;
using Connected.DAL.Configuration.Repositories;
using Connected.DAL.Core.Configuration.Model;
using Connected.Schemas;
using Connected.Schemas.Common;

namespace Connected.Dispatcher
{
    public class DispatcherService : IDispatcherService
    {
        public IMessageStorage MessageStorage { get; set; }
        public IConnectedConfDBRepository ConnectedConfDBRepository{ get; set; }

        private readonly int _messageLimitToBoMoved;
        private readonly int _messageLimitToBeDispatched;
        private readonly int _waitMiliSec = 5000;

        private readonly CancellationTokenSource _ts;
        private CancellationToken _ct;

        private List<AdapterBasicDTO> _receiveAdaptersWithDetails;

        private int _waitingListLimit;
        public DispatcherService(IMessageStorage messageStorage, IConnectedConfDBRepository connectedConfDBRepository)
        {
            //Task cancellation token
            _ts = new CancellationTokenSource();
            _ct = _ts.Token;

            //TODO : Get this from app config or constructor
            //TODO : Set this a higher value
            _messageLimitToBoMoved = 10;
            _messageLimitToBeDispatched = 10;

            MessageStorage = messageStorage;
            ConnectedConfDBRepository = connectedConfDBRepository;

            //Get receive adapters 
            _receiveAdaptersWithDetails = ConnectedConfDBRepository.GetReceiveAdaptersWithDetails();

            //TODO : Get this from app config
            //Set waiting list limit
            _waitingListLimit = 25;
        }
       
        //TODO : ???
        ~DispatcherService()
        {
            this.Close();
        }

        //TODO : Find a better solution
        public void Close()
        {
            _ts.Cancel();
        }

        /// <summary>
        /// </summary>
        public Task StartDispatcherService()
        {
            //Get all receive adapters 
            _receiveAdaptersWithDetails = ConnectedConfDBRepository.GetReceiveAdaptersWithDetails();

            //Start first task of Dispatcher Service
            var task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var result = MoveMessagesToMessagesRouterFinder();
                    //TODO : Log if result is false

                    if (_ct.IsCancellationRequested) break;

                    Task.Delay(_waitMiliSec, _ct).Wait(_ct);
                }
            }, _ct);

            //Start second task of Dispatcher Service : Push messages to Receive Adapters 
            var task2 = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        PushMessages();
                    }
                    catch (Exception)
                    {
                        //TODO : Log if result is false
                    }

                    if (_ct.IsCancellationRequested) break;

                    Task.Delay(_waitMiliSec, _ct).Wait(_ct);
                }
            }, _ct);

            return task;
        }

        /// <summary>
        /// Sends the message to MessagesRouterFinder topic for the Route Finder to 
        /// evaluate the message against a list of registered subscribers and any subscription criteria
        /// </summary>
        public bool MoveMessagesToMessagesRouterFinder()
        {
            return MessageStorage.MoveMessagesToMessagesRouterFinder(_messageLimitToBoMoved);
        }

        /// <summary>
        /// MessageTimeCount
        /// </summary>
        private readonly List<Tuple<MessageEnriched, DateTime, int>> _waitingList = new List<Tuple<MessageEnriched, DateTime,int>>();

        /// <summary>
        /// TODO : Fill this
        /// </summary>
        public void PushMessages()
        {
            //Get messages to push them 
            List<MessageEnriched> messagesEnriched = new List<MessageEnriched>();

            //If there are waiting messages
            var waitingListReTryList = _waitingList.Where(x => x.Item2 < DateTime.Now).ToList();

            if (waitingListReTryList.Any())
            {
                //Add wating list
                messagesEnriched.AddRange(waitingListReTryList.Select(x => x.Item1).ToList());
            }

            //Add more until limit
            //Check if waiting list reached to the limit, then dont take from DB 
            int messageCountToTakeFromDB = _waitingList.Count >= _waitingListLimit ? 0 : 
                (_messageLimitToBeDispatched - waitingListReTryList.Count() > 0
                ? _messageLimitToBeDispatched - waitingListReTryList.Count()
                : 0);
            var messagesToBeDispatched = MessageStorage.GetMessagesToBeDispatched(messageCountToTakeFromDB,
                waitingListReTryList.Count());
            messagesEnriched.AddRange(messagesToBeDispatched);

            //Group messages by receive adapter 
            var messagesGrouped = messagesEnriched.GroupBy(x => x.ReceiveAdapterId).ToList();
            foreach (var subMessages in messagesGrouped)
            {
                var receiveAdapterId = subMessages.FirstOrDefault().ReceiveAdapterId;
                var receiveAdapter = _receiveAdaptersWithDetails.FirstOrDefault(x => x.Id == receiveAdapterId);
                if (receiveAdapter == null) continue;

                //Receive adapter url
                var uri = receiveAdapter.ReceiveAdapterDetails.AdapterServiceURI;

                //Create channel factory for this receive adapter
                var channelFactory = Connected.Common.ClientWCFServiceInvokerUtil.CreateReceiveAdapterServiceChannelFactory(uri);

                //Send messages 
                foreach (var messageEnriched in subMessages)
                {
                    int timestoRetry = receiveAdapter.ReceiveAdapterDetails.TimesToRetry ?? 0;
                    int retryInterval = receiveAdapter.ReceiveAdapterDetails.RetryInterval ?? 0;
                    
                    try
                    {
                        TransferMessage transferMessages = new TransferMessage()
                        {
                            Header = new TransferMessageHeader()
                            {
                                Destination = "Receive Adapter",
                                MessageType = "Pushed Message",
                                Source = "Dispatcher",
                                Status = "From Dispatcher",
                                TimeStamp = DateTime.Now.ToString()
                            },
                            Body =
                                SerializationHelper.SerializeObjectToXml<string>(messageEnriched.Message).DocumentElement
                        };

                        Connected.Common.ClientWCFServiceInvokerUtil.CallReceiveAdapterWcfService(channelFactory, transferMessages);
                        
                        //Delete if succesfully sent
                        MessageStorage.RemoveMessagesDispatcher(messageEnriched.Id);
                    }
                    catch (Exception)
                    {
                        //TODO : Log message
                        var waitingMessage = _waitingList.FirstOrDefault(x => x.Item1.Id == messageEnriched.Id);
                        if (waitingMessage != null)
                        {
                            if (waitingMessage.Item3 >= receiveAdapter.ReceiveAdapterDetails.TimesToRetry)
                            {
                                //TODO : Send message back to inbound service as failed message
                                //TODO : remove from dispatcher topic 

                                //Remove from waiting list
                                _waitingList.Remove(waitingMessage);
                            }
                            else
                            {
                                //TODO : Find a better solution to update Tuple
                                _waitingList.Remove(waitingMessage);
                                _waitingList.Add(new Tuple<MessageEnriched, DateTime, int>(
                                    waitingMessage.Item1,
                                    DateTime.Now.AddMilliseconds(receiveAdapter.ReceiveAdapterDetails.RetryInterval ?? 0), 
                                    waitingMessage.Item3 + 1));
                            }
                        }
                        else
                        {
                            //If re try is 0 
                            if (receiveAdapter.ReceiveAdapterDetails.TimesToRetry == 0)
                            {
                                //TODO : Send message back to inbound service as failed message
                                //TODO : remove from dispatcher topic 

                                //Remove from waiting list
                                _waitingList.Remove(waitingMessage);
                            }
                                //Add to waiting list 
                            else
                            {
                                _waitingList.Add(
                                   new Tuple<MessageEnriched, DateTime, int>(
                                       messageEnriched,
                                       DateTime.Now.AddMilliseconds(receiveAdapter.ReceiveAdapterDetails.RetryInterval ?? 0),
                                       1));
                            }
                        }
                    }
                }

                //Close channel factory
                channelFactory.Close();
            }
        }
    }
}
