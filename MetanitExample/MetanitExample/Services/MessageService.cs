using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample.Services
{
    public class MessageService
    {
        IMessageSender sender;

        public MessageService(IMessageSender sender)
        {
            this.sender = sender;
        }

        public string Send()
        {
            return sender.Send();
        }
    }
}
