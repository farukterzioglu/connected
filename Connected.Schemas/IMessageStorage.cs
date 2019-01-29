using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Schemas
{
    public interface IMessageStorage
    {
        void InsertMessage(Message message);
        void InsertMessages(List<Message> messages);

        List<Message> GetAllMessages();
        List<Message> GetMessages(int count);

        //bool DeleteMessages(List<int> messageIds, out List<int> notDeletedMessageIds);

        /// <summary>
        /// Sends the message to MessagesRouterFinder topic for the Route Finder to 
        /// evaluate the message against a list of registered subscribers and any subscription criteria
        /// </summary>
        /// <param name="messageLimit"></param>
        /// <returns></returns>

        #region for dispatcher
        //To Route Finder
        bool MoveMessagesToMessagesRouterFinder(int messageLimit);

        //Dispatching 
        List<MessageEnriched> GetMessagesToBeDispatched(int? limit = null);
        List<MessageEnriched> GetMessagesToBeDispatched(int? limit, int passN = 0);
        bool RemoveMessagesDispatcher(int messageId);
        //bool RemoveMessagesDispatcher(List<int> messageIds);
        #endregion 

        #region for raoute finder   
        bool MoveFromMessagesRouterFinderToMessageDispatcher(int limit,
            List<Tuple<int, int>> messageTypeIdReceiveAdapterIdPair);
        #endregion
    }
}
