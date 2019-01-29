using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.MessageStorage.MessageQueue;
using Connected.Schemas;

namespace Connected.MessageStorage
{
    public class LoggedMessagesDecorator : MessageStorageDecorator
    {
        public LoggedMessagesDecorator(IMessageStorage messageStorageToBeDecorated)
            : base(messageStorageToBeDecorated)
        {
        }

        public override void InsertMessage(Message message)
        {
            base.InsertMessage(message);

            new Connected.Common.LogManagement.DbLoger().WriteLog(message.MessageText);
        }

        public override void InsertMessages(List<Message> messages)
        {
            base.InsertMessages(messages);

            string temp = messages.Aggregate("", (current, message) => current + " | " + message.MessageText);
            new Connected.Common.LogManagement.DbLoger().WriteLog(temp);
        }
    }
}
