using Business_Tier.Internal_service.Communication_Service;
using Interface_Tier.Service;
using Interface_Tier.Service.Communication_Service;
using Interface_Tier.Service.Internal_service.Communication_Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Communication_Service
{
    public class WebSocketHandlerService(IUserAuthorizationService userAuthorization ,
        IWebSocketConnectionsService connectionsService, IProjectChatMessageService projectMessageService, ITaskChatMessageService chatMessageService) : IWebSocketHandlerService
    {
      
        
        public async Task HandleAsync(HttpContext context, WebSocket socket)
        {
            int UserID = int.Parse(context.User.FindFirst("userId")?.Value);
            int GroupID = -1;
            var ProjectID = context.Request.Query["projectId"];
            var TaskID = context.Request.Query["taskId"];
            WebSocketMessagingServiceBase webSocketMessaging = null;

            if (ProjectID.Any())
            {
                GroupID = int.Parse(ProjectID);
                bool IsAuthorized = await userAuthorization.IsUserAuthorizedForProjectChat(UserID, GroupID);
                if (!IsAuthorized)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Unauthorized", CancellationToken.None);
                    return;
                }
                connectionsService.AddUserSocketToProjectGroup(UserID, GroupID, socket);


                webSocketMessaging = new ProjectWebSocketMessagingService(connectionsService, projectMessageService);

            }
            else if (TaskID.Any())
            {
                GroupID = int.Parse(TaskID);
                bool IsAuthorized = await userAuthorization.IsUserAuthorizedForTaskChat(UserID, GroupID);
                if (!IsAuthorized)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Unauthorized", CancellationToken.None);
                    return;
                }
                connectionsService.AddUserSocketToTaskGroup(UserID, GroupID, socket);
                webSocketMessaging = new TaskWebSocketMessagingService(connectionsService, chatMessageService);

            }

            if (GroupID != -1 && webSocketMessaging != null)
            {
                //await webSocketMessaging.BroadcastToGroupAsync(UserID, GroupID, "new user joined");
                await webSocketMessaging.ReceiveFromGroupAsync(UserID, GroupID);
            }
            else
            {
                await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Missing projectId or TaskId", CancellationToken.None);
                return;
            }
        }

      
    }
}
