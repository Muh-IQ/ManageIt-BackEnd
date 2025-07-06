using Interface_Tier.Service;
using Interface_Tier.Service.Internal_service.Communication_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Internal_service.Communication_Service
{
    public class ProjectWebSocketMessagingService(IWebSocketConnectionsService connectionsService , IProjectChatMessageService messageService) : WebSocketMessagingServiceBase
    {
        public override async Task<bool> AddMessageToDB(int userId, int groupId, string message)
        {
            return await messageService.AddMessage(userId, groupId, message);
        }

        public override void CloseConnection(int userId, int groupId)
        {
            connectionsService.CloseUserProjectConnection(userId, groupId);
        }

        public override Dictionary<int, WebSocket> GetGroup(int groupId)
        {
            return connectionsService.GetProjectGroup(groupId);
        }

        public override WebSocket GetSocket(int userId, int groupId)
        {
            return connectionsService.GetUserProject(userId, groupId);
        }
       
    }
}
