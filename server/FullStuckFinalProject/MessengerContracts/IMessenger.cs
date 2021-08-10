using MessengerContracts;
using ProjectDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectContracts
{
    public interface IMessenger
    {
        Task Send(string i_ID, object i_Message);
        Task Broadcast(string i_ID, MessageMarker i_Message);
        Task<IReceiver> Add(string i_ID, ISocket i_Socket);
    }
}
