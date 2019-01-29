using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Schemas;

namespace Connected.MessageStorage
{
    public class MessageStorageDecorator : IMessageStorage
    {
        private readonly IMessageStorage _messageStorageToBeDecorated;

        public MessageStorageDecorator(IMessageStorage messageStorageToBeDecorated)
        {
            _messageStorageToBeDecorated = messageStorageToBeDecorated;
        }

        public virtual void  InsertMessage(Message message)
        {
            _messageStorageToBeDecorated.InsertMessage(message);
        }

        public virtual void InsertMessages(List<Message> messages)
        {
            _messageStorageToBeDecorated.InsertMessages(messages);
        }

        public List<Message> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessages(int count)
        {
            throw new NotImplementedException();
        }

        public List<MessageEnriched> GetMessagesToBeDispatched()
        {
            return _messageStorageToBeDecorated.GetMessagesToBeDispatched();
        }

        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit, int passN = 0)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMessagesDispatcher(int messageId)
        {
            return _messageStorageToBeDecorated.RemoveMessagesDispatcher(messageId);
        }

        public bool MoveFromMessagesRouterFinderToMessageDispatcher(int limit, List<Tuple<int, int>> messageTypeIdReceiveAdapterIdPair)
        {
            throw new NotImplementedException();
        }

        public bool MoveMessagesToMessagesRouterFinder(int messageLimit)
        {
            return _messageStorageToBeDecorated.MoveMessagesToMessagesRouterFinder(messageLimit);
        }

        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit = null)
        {
            throw new NotImplementedException();
        }
    }
}
