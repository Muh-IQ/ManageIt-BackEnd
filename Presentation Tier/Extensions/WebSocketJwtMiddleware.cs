using Business_Tier.Internal_service.Auths_Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Presentation_Tier.Extensions
{
    public class WebSocketJwtMiddleware(RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var token = context.Request.Query["access_token"].FirstOrDefault();

                if (string.IsNullOrEmpty(token) || !ValidateToken(token, context))
                {
                    context.Response.StatusCode = 401; 
                    await context.Response.WriteAsync("Invalid or missing token.");
                    return;
                }
            }

            await next(context);
        }

        private bool ValidateToken(string token, HttpContext context)
        {
            AuthsCredentials credentials = AuthsCredentials.Instance;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(credentials.SecretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = credentials.Issuer,
                    ValidateAudience = true,
                    ValidAudience = credentials.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                context.User = principal;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
