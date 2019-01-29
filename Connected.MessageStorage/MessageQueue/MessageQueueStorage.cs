using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.MessageStorage.MessageQueue;
using Connected.Schemas;
using Infrastructure.AspectOriented.Aspects;

namespace Connected.MessageStorage
{
    public class MessageQueueStorage : IMessageStorage
    {
        [TraceAspect]
        public void InsertMessage(Message message)
        {
            InsertMessages(new List<Message>() {message});
        }
        public void InsertMessages(List<Message> messages)
        {
            using (var context = new MessageQueue.MessageQueueEntities())
            {
                context.Messages.AddRange(
                    messages.Select(x => new MessageQueue.Messages()
                    {
                        DateTime = x.Datetime,
                        Message = x.MessageText,
                        MessageTypeId = x.MessageType
                    }).ToList());
                context.SaveChanges();
            }
        }

        public List<Message> GetAllMessages()
        {
            using (var context = new MessageQueue.MessageQueueEntities())
            {
                return context.Messages.Select( x=> new Message()
                {
                    Datetime = x.DateTime,
                    MessageText = x.Message,
                    MessageType = x.MessageTypeId
                }).ToList();
            }
        }
        public List<Message> GetMessages(int count)
        {
            using (var context = new MessageQueue.MessageQueueEntities())
            {
                return context.Messages
                    .Select(x => new Message()
                    {
                        Datetime = x.DateTime,
                        MessageText = x.Message,
                        MessageType = x.MessageTypeId
                    })
                    .Take(count)
                    .ToList();
            }
        }

        #region For dispatcher
        public bool MoveMessagesToMessagesRouterFinder(int messageLimit)
        {
            try
            {
                //TODO : User stored procedure instead of this 
                using (var ctx = new MessageQueue.MessageQueueEntities())
                {
                    var messagesToMove = ctx.Messages.Take(messageLimit).ToList();

                    ctx.MessagesRouteFinder.AddRange(
                        messagesToMove.Select(x =>
                            new MessageQueue.MessagesRouteFinder()
                            {
                                DateTime = x.DateTime,
                                Message = x.Message,
                                MessageTypeId = x.MessageTypeId
                            }));

                    ctx.Messages.RemoveRange(messagesToMove);

                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit = null)
        {
            using (var db = new MessageQueue.MessageQueueEntities())
            {
                var query = db.MessagesDispatcher.Select(x => new MessageEnriched()
                {
                    Datetime = x.DateTime,
                    Message = x.Message,
                    ReceiveAdapterId = x.ReceiveAdapterId
                });
                if (limit != null)
                    query = query.Take((int)limit);

                return query.ToList();
            }
        }
        public List<MessageEnriched> GetMessagesToBeDispatched(int? limit, int passN = 0)
        {
            using (var db = new MessageQueue.MessageQueueEntities())
            {
                var query = db.MessagesDispatcher.Select(x => new MessageEnriched()
                {
                    Id = x.Id,
                    Datetime = x.DateTime,
                    Message = x.Message,
                    ReceiveAdapterId = x.ReceiveAdapterId
                }).OrderBy(x => x.Id).Skip(passN);

                if (limit != null)
                    query = query.Take((int)limit);

                return query.ToList();
            }
        }
        
        public bool RemoveMessagesDispatcher(int messageId)
        {
            using (var db = new MessageQueue.MessageQueueEntities())
            {
                try
                {
                    var employer = new MessageQueue.MessagesDispatcher {Id = messageId};
                    db.MessagesDispatcher.Attach(employer);
                    db.MessagesDispatcher.Remove(employer);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        #region For router finder 
        public bool MoveFromMessagesRouterFinderToMessageDispatcher(int limit, List<Tuple<int, int>> messageTypeIdReceiveAdapterIdPair)
        {
            using (var ctx = new MessageQueue.MessageQueueEntities())
            {
                //Get message types that has subscriber
                List<int> messageTypes = messageTypeIdReceiveAdapterIdPair.Select( x=> x.Item1).Distinct().ToList();

                //Select messages from Route Finder Topic that has Subscription
                List<MessageQueue.MessagesRouteFinder> messagesToMove = ctx.
                    MessagesRouteFinder
                    .Where(x =>messageTypes.Contains( x.MessageTypeId )).
                    Take(limit)
                    .ToList();

                //Inser to Message Dispatcher
                List<MessagesDispatcher> enrichedMessages = new List<MessagesDispatcher>();
                //Select a message type
                foreach (int messageType in messageTypes)
                {
                    //Get receive adapter for this message type 
                    List<int> receiveAdapterIds =
                        messageTypeIdReceiveAdapterIdPair.Where(x => x.Item1 == messageType).Select( x=> x.Item2).ToList();

                    //Get messages for this message type 
                    var messagesToEnrich = messagesToMove.Where( x=> x.MessageTypeId == messageType).ToList();

                    foreach (var message in messagesToEnrich)
                    {
                        //enrich message per receive adapter
                        foreach (int receiveAdapterId in receiveAdapterIds)
                        {
                            enrichedMessages.Add(new MessageQueue.MessagesDispatcher()
                            {
                                DateTime = message.DateTime,
                                Message = message.Message,
                                ReceiveAdapterId = receiveAdapterId
                            });
                        }
                    }
                }
                    
                ctx.MessagesDispatcher.AddRange(enrichedMessages);

                //Remove from Route Finder Topic 
                ctx.MessagesRouteFinder.RemoveRange(messagesToMove);

                //Save Changes 
                ctx.SaveChanges();
            }
            return true;
        }
        #endregion 
    }
}
