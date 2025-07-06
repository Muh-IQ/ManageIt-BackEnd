using Interface_Tier.Service.Internal_service.Communication_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business_Tier.Internal_service.Communication_Service
{
    public abstract class WebSocketMessagingServiceBase 
    {
        public abstract Dictionary<int, WebSocket> GetGroup(int groupId);
        public abstract WebSocket GetSocket(int userId, int groupId);
        public abstract void CloseConnection(int userId, int groupId);
        public abstract Task<bool> AddMessageToDB(int userId, int groupId, string message);
        public async Task BroadcastToGroupAsync(int senderUserId, int groupId, string message)
        {

            bool IsAdd = await AddMessageToDB(senderUserId,groupId, message);

            if (!IsAdd)
                return;


            Dictionary<int, WebSocket> group = GetGroup(groupId);

            var payload = new
            {
                userId = senderUserId,
                message = message
            };

            string json = JsonSerializer.Serialize(payload);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var (userId, socket) in group)
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        if (!(socket.State == WebSocketState.Open))
                        {
                            // close connection
                            CloseConnection(userId, groupId);
                        }
                        else
                        {
                            // rigiset this exption in logger file
                        }
                    }
                }
                else
                {
                    // close connection
                    CloseConnection(userId, groupId);

                }
            }
        }
        public async Task ReceiveFromGroupAsync(int userId, int groupId)
        {
            var buffer = new byte[1024 * 4];
            WebSocket socket = GetSocket(userId, groupId);

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);

                    // close connection which mean , I remove this user form Project or Task collection
                    CloseConnection(userId, groupId);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastToGroupAsync(userId, groupId, message);
                }
            }
        }
    }
}
