using Interface_Tier.Service.Internal_service.Communication_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Internal_service.Communication_Service
{
    public class WebSocketConnectionsService : IWebSocketConnectionsService
    {
        private  Dictionary<int, Dictionary<int, WebSocket>> ProjectsGroups = new();
        private  Dictionary<int, Dictionary<int, WebSocket>> TasksGroups = new();

        public void AddUserSocketToProjectGroup(int UserID, int PorjectId, WebSocket socket)
        {
            if (ProjectsGroups.ContainsKey(PorjectId))
            {
                if (!ProjectsGroups[PorjectId].ContainsKey(UserID))
                {
                    ProjectsGroups[PorjectId][UserID] = socket;
                }
                else
                    ProjectsGroups[PorjectId].Add(UserID, socket);

            }
            else
            {
                ProjectsGroups.Add(PorjectId, new Dictionary<int, WebSocket> { { UserID, socket } });
            }
        }

        public void AddUserSocketToTaskGroup(int UserID, int TaskId, WebSocket socket)
        {
            if (TasksGroups.ContainsKey(TaskId))
            {
                if (!TasksGroups[TaskId].ContainsKey(UserID))
                {
                    TasksGroups[TaskId][UserID] = socket;
                }
                else
                    TasksGroups[TaskId].Add(UserID, socket);

            }
            else
            {
                TasksGroups.Add(TaskId, new Dictionary<int, WebSocket> { { UserID, socket } });
            }
        }

        public void CloseUserProjectConnection(int userId, int groupId)
        {
            ProjectsGroups[groupId]?.Remove(userId);
        }

        public void CloseUserTaskConnection(int userId, int groupId)
        {
            TasksGroups[groupId]?.Remove(userId);
        }

        public Dictionary<int,  WebSocket> GetProjectGroup(int groupId)
        {
            return ProjectsGroups[groupId];
        }

        public Dictionary<int, WebSocket> GetTaskGroup(int groupId)
        {
            return TasksGroups[groupId];
        }
        public WebSocket GetUserProject(int userId, int groupId)
        {
            return ProjectsGroups[groupId][userId];
        }
        public WebSocket GetUserTask(int userId, int groupId)
        {
            return TasksGroups[groupId][userId];
        }
    }
}
