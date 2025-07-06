using Business_Tier;
using Business_Tier.Cloudinary_Service;
using Business_Tier.Communication_Service;
using Business_Tier.Email_Service;
using Business_Tier.Internal_service.Auths_Service;
using Business_Tier.Internal_service.Communication_Service;
using Business_Tier.Internal_service.Cryptography_service;
using Data_Access_Tier;
using Interface_Tier.Repository;
using Interface_Tier.Service;
using Interface_Tier.Service.Communication_Service;
using Interface_Tier.Service.Internal_service.Auths_Service;
using Interface_Tier.Service.Internal_service.Communication_Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation_Tier.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrdinaryServices(this IServiceCollection Services)
        {
            Services.AddTransient<IUserService, UserService>();
            Services.AddTransient<IUserRepository, UserRepository>();



            Services.AddTransient<IOTPService, OTPService>();
            Services.AddTransient<IOTPRepository, OTPRepository>();

            Services.AddTransient<IProjectRepository, ProjectRepository>();
            Services.AddTransient<IProjectService, ProjectService>();

            Services.AddTransient<IProjectMemberService, ProjectMemberService>();
            Services.AddTransient<IProjectMemberRepository, ProjectMemberRepository>();

            Services.AddTransient<IProjectChatMessageService, ProjectChatMessageService>();
            Services.AddTransient<IProjectChatMessageRepository, ProjectChatMessageRepository>();

            Services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
            Services.AddTransient<IUserAuthorizationRepository, UserAuthorizationRepository>();

            Services.AddTransient<IProjectChatMemberService, ProjectChatMemberService>();
            Services.AddTransient<IProjectChatMemberRepository, ProjectChatMemberRepository>();

            Services.AddTransient<ITaskListService, TaskListService>();
            Services.AddTransient<ITaskListRepository, TaskListRepository>();

            Services.AddTransient<ITaskService, TaskService>();
            Services.AddTransient<ITaskRepository, TaskRepository>();

            Services.AddTransient<ITaskMemberService, TaskMemberService>();
            Services.AddTransient<ITaskMemberRepository, TaskMemberRepository>();

            Services.AddTransient<ITaskPermissionService, TaskPermissionService>();

            Services.AddTransient<ITaskChatMessageService, TaskChatMessageService>();
            Services.AddTransient<ITaskChatMessageRepository, TaskChatMessageRepository>();
            return Services;
        }

        public static IServiceCollection AddInternalServices(this IServiceCollection Services)
        {
            Services.AddTransient<ICryptographyService, CryptographyService>();
            Services.AddSingleton<IAccessTokenService, AccessTokenService>();
            Services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            Services.AddSingleton<IWebSocketConnectionsService, WebSocketConnectionsService>(); //Singleton
            Services.AddTransient<IWebSocketHandlerService, WebSocketHandlerService>();

            return Services;
        }

        public static IServiceCollection AddThirdPartyServices(this IServiceCollection Services)
        {
            Services.AddTransient<IEmailService, SendGridService>();
            Services.AddTransient<IImageService, CloudinaryService>(); 
            return Services;
        }
        public static IServiceCollection AddJWTServices(this IServiceCollection Services)
        {
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AuthsCredentials.Instance.Issuer,
                    ValidAudience = AuthsCredentials.Instance.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthsCredentials.Instance.SecretKey))
                };
            });

            return Services;
        }
    }
}
