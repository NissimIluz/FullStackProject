using InfraAttributes;
using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adapters
{
    public class Receiver : IReceiver
    {
        //Add for commit receiver for future use 
        Task m_RecTask;
        WebSocket m_WebSocket;
        public Receiver(WebSocket webSocket)
        {
            m_WebSocket = webSocket;
        }
        public void Start()
        {
            m_RecTask = new Task(async () =>
            {
                for (; ; )
                {
                    var buffer = new byte[4096];
                    var response = await m_WebSocket.ReceiveAsync(new Memory<byte>(buffer), CancellationToken.None);
                    if (response.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(buffer));
                    }
                }
            });
            m_RecTask.Start();
        }
    }
}
