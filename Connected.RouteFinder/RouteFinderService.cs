using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Connected.DAL.Configuration.Repositories;
using Connected.Schemas;

namespace Connected.RouteFinder
{
    public class RouteFinderService :  IRouteFinderService
    {
        public IMessageStorage MessageStorage { get; set; }
        public IConnectedConfDBRepository ConnectedConfDBRepository{ get; set; }

        //TODO : NInject this
        Connected.Common.LogManagement.ILoger _loger = new Connected.Common.LogManagement.EventLoger();

        private List<Connected.DAL.Core.Configuration.Model.MessageTypeDTO> _messageTypes;
        private List<Connected.DAL.Core.Configuration.Model.MessageSubscriptionDetailsDTO> _messageSubscriptionDetails;
        private readonly int _messageLimitToBoEnriched ;

        private readonly CancellationTokenSource _ts;
        private CancellationToken _ct;

        //TODO : Get this from connected settings 
        private readonly int _waitMiliSec = 1000;

        public RouteFinderService(IMessageStorage messageStorage, IConnectedConfDBRepository connectedConfDBRepository)
        {
            //Task cancellation token
            _ts = new CancellationTokenSource();
            _ct = _ts.Token;

            //TODO : Get this from app config or constructor
            //TODO : Set this a higher value
            _messageLimitToBoEnriched = 10;

            MessageStorage = messageStorage;
            ConnectedConfDBRepository = connectedConfDBRepository;

            _messageTypes = ConnectedConfDBRepository.GetMessageTypes();
            _messageSubscriptionDetails = ConnectedConfDBRepository.GetMessageSubscriptionDetails();
        }

        //TODO : ???
        ~RouteFinderService()
        {
            this.Close();
        }

        //private Dictionary<int, int> _messageTypeIdReceiveAdapterIdPair;
        private List<Tuple<int, int>> _messageTypeIdReceiveAdapterIdPair;
        private void GetSubScriptions()
        {
            _messageTypeIdReceiveAdapterIdPair =
                ConnectedConfDBRepository.GetMessageSubscriptionDetails()
                    .Select(x => new Tuple<int, int>(x.MessageTypeId, x.AdapterId)).ToList();
        }

        public Task StartRouteFinderService()
        {
            //Get subsciptions
            GetSubScriptions();

            var task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    RouteMessages();

                    if (_ct.IsCancellationRequested) break;

                    Task.Delay(_waitMiliSec, _ct).Wait(_ct);
                }
            }, _ct);

            return task;
        }

        public bool RouteMessages()
        {
            try
            {
                if (_messageTypeIdReceiveAdapterIdPair == null)
                    GetSubScriptions();

                var result = MessageStorage.MoveFromMessagesRouterFinderToMessageDispatcher(_messageLimitToBoEnriched,_messageTypeIdReceiveAdapterIdPair);

                if (!result)
                    throw new Exception("Couldn't route messages. Please see other logs.");

                return true;
            }
            catch (Exception ex)
            {
                _loger.WriteLog(
                    "Routing error! (" + ex.Message + ")",
                    global::Common.Enums.LogType.Error, "Route Finder");

                return false;
            }
        }

        //TODO : Find a better solution
        public void Close()
        {
            _ts.Cancel();
        }
    }
}
