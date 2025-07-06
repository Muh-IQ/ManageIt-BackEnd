using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service.Internal_service.Communication_Service
{
    public interface IWebSocketConnectionsService
    {
        Dictionary<int, WebSocket> GetTaskGroup(int groupId);
        Dictionary<int, WebSocket> GetProjectGroup(int groupId);
        void AddUserSocketToProjectGroup(int UserID, int PorjectId, WebSocket socket);
        void AddUserSocketToTaskGroup(int UserID, int TaskId, WebSocket socket);
        WebSocket GetUserProject(int userId, int groupId);
        WebSocket GetUserTask(int userId, int groupId);
        public void CloseUserProjectConnection(int userId, int groupId);
        public void CloseUserTaskConnection(int userId, int groupId);
    }
}
