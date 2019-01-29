using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Schemas;
using Infrastructure.AspectOriented.Aspects;

namespace Connected.MessageStorage.Fake
{
    public class FakeStorage : IMessageStorage
    {
        private readonly List<Message> _messageList = new List<Message>();
        private readonly List<Message> _routerFinderList = new List<Message>();

        [TraceAspect]
        public void InsertMessage(Message message)
        {
            _messageList.Add(message);
        }

        [TraceAspect]
        public void InsertMessages(List<Message> messages)
        {
            foreach (Message message in _messageList)
            {
                InsertMessage(message);
            }
        }

        [TraceAspect]
        public List<Message> GetAllMessages()
        {
            return _messageList.ToList();
        }

        [TraceAspect]
        public List<Message> GetMessages(int count)
        {
            return _messageList.Take(count).ToList();
        }

        public List<MessageEnriched> GetMessagesToBeDispatched()
        {
            throw new NotImplementedException();
        }

        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit, int passN = 0)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMessagesDispatcher(int messageId)
        {
            throw new NotImplementedException();
        }

        public bool MoveFromMessagesRouterFinderToMessageDispatcher(int limit, List<Tuple<int, int>> messageTypeIdReceiveAdapterIdPair)
        {
            throw new NotImplementedException();
        }

        public bool MoveMessagesToMessagesRouterFinder(int messageLimit)
        {
            try
            {
                _routerFinderList.AddRange(_messageList.Take(messageLimit));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit = null)
        {
            throw new NotImplementedException();
        }

        public bool InsertToMessagesDispatcher(MessageEnriched messageEnriched)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromMessagesRouteFinder(int messagesRouteFinderId)
        {
            throw new NotImplementedException();
        }

        //Fake storage implementation
        public List<Message> GetRouteMessages()
        {
            return _routerFinderList;
        }
    }
}
