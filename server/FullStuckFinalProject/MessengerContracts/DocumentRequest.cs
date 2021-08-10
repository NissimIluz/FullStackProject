using ProjectContracts;
using ProjectDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerContracts
{
    public class MessageDocument // can be change to whom send it and to who
    {
        public string Action { get; set; }
        public DocumentDTO DocumentDTO { get; set; }
    }

    public class DocumentRequest //Message request from client - ID of client and message body 
    {
        public string ID { get; set; }

        public MessageDocument MessageBody { get; set; }
    }

}
