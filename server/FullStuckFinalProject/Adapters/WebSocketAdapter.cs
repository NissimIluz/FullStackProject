using InfraAttributes;
using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Adapters
{
    [Register(typeof(ISocket))]
    [Policy(Policy.Singleton)]
    public class WebSocketAdapter : ISocket
    {
        public WebSocket Socket { get; set; }
    }
}
