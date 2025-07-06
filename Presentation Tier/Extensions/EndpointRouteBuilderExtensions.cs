using Interface_Tier.Service.Communication_Service;

namespace Presentation_Tier.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapWebSocketEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var handler = context.RequestServices.GetRequiredService<IWebSocketHandlerService>();
                    await handler.HandleAsync(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });
        }
    }
}
