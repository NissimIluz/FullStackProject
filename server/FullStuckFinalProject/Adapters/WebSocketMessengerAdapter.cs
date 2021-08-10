using Newtonsoft.Json;
using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessengerContracts;
using Microsoft.Extensions.Logging;
using InfraAttributes;

namespace Adapters
{
    [Register(typeof(IMessenger))]
    [Policy(Policy.Singleton)]
    public class WebSocketMessengerAdapter : IMessenger
    {
        private readonly Dictionary<string, WebSocket> r_SocketsDictionary;
        private readonly Dictionary<string, Receiver> r_ReceiverssDictionary; // In the future the server can receive messages and not only to send them
        private readonly ILogger<WebSocketMessengerAdapter> r_Logger;
        private readonly Dictionary<string, HashSet<string>> r_documentSocket;
        private readonly Dictionary<string, string> r_forDeleteSocker;

        public WebSocketMessengerAdapter(ILogger<WebSocketMessengerAdapter> i_Logger)
        {
            r_Logger = i_Logger;
            r_SocketsDictionary = new Dictionary<string, WebSocket>();
            r_documentSocket = new Dictionary<string, HashSet<string> >();
            r_forDeleteSocker = new Dictionary<string, string>();
        }
        public async Task Send(string i_ID, object i_MessageBody) // Send message to other socket
        {
            if (r_SocketsDictionary.ContainsKey(i_ID))
            {
               string message = JsonConvert.SerializeObject(i_MessageBody); //Convert message to JSON
                var buffer = Encoding.UTF8.GetBytes(message); // Convert JSON to BYTES
                await r_SocketsDictionary[i_ID].SendAsync(new ReadOnlyMemory<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None); //Send the message as TEXT
            }
        }

        public async Task Broadcast(string id, MessageMarker i_MessageBody)
        {
            
            var socketsIdList = r_documentSocket[id];
            foreach(var sockutID in socketsIdList)
            {
              if (!sockutID.Equals(i_MessageBody.MarkerDTO.UserID+"_doc"))
                await Send(sockutID, i_MessageBody);
            }
        }

        public async Task<IReceiver> Add(string i_ID, ISocket i_Socket) // Add ID to the socket 
        {
            WebSocketAdapter webSocketAdapter = i_Socket as WebSocketAdapter;
            Receiver retval = null;
            var arrID = i_ID.Split(',');    //userId,docId
            string socketID = arrID[0];  // if else
            if (r_SocketsDictionary.ContainsKey(socketID)) //if socket already has ID we will delete the old one and replace it with the new one
            {
                var cursocket = r_SocketsDictionary[socketID];
                r_SocketsDictionary.Remove(socketID);
                if (arrID.Length > 1)
                {
                    string documentID = arrID[1];
                    r_documentSocket[r_forDeleteSocker[socketID]].Remove(socketID);
                    r_forDeleteSocker.Remove(socketID);
                }

                }
            r_SocketsDictionary.Add(socketID, webSocketAdapter.Socket);

            if(arrID.Length >1)
            {
                string documentID = arrID[1];
                if (!r_documentSocket.ContainsKey(documentID))               
                    r_documentSocket.Add(documentID, new HashSet<string>());
                r_documentSocket[documentID].Add(socketID);
                r_forDeleteSocker.Add(socketID, documentID);
            }        
            retval = new Receiver(webSocketAdapter.Socket); // Create a new receiver and return it
            return retval;
        }
    }
}
