using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service.Communication_Service
{
    public interface IWebSocketHandlerService
    {
        Task HandleAsync(HttpContext context, WebSocket socket);

    }
}
