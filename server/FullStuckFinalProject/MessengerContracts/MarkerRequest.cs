using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerContracts
{
    public class MessageMarker // can be change to whom send it and to who
    {
        public string Action { get; set; }
        public Marker MarkerDTO { get; set; }
    }

    public class MarkerRequest //Message request from client - ID of client and message body 
    {
        public string ID { get; set; }

        public MessageMarker MessageBody { get; set; }
    }
}
