using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample.Services
{
    public class SmsMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Sent to SMS";
        }
    }
}
