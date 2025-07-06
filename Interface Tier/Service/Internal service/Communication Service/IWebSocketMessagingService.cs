using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service.Internal_service.Communication_Service
{
    public interface IWebSocketMessagingService
    {
        Task BroadcastToGroupAsync(int groupId, string message);
        Task ReceiveFromGroupAsync(int userId, int groupId, WebSocket socket);
    }
}
