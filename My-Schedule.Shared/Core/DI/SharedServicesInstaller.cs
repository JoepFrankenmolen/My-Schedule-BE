using My_Schedule.Shared.DTO.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Middleware;
using My_Schedule.Shared.RabbitMQ;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services;
using My_Schedule.Shared.Services.Authorization;
using My_Schedule.Shared.Services.Authorization.Interfaces;
using My_Schedule.Shared.Services.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.Shared.Services.Users.Users;
using My_Schedule.Shared.Services.Users.UserSecurities;

namespace My_Schedule.Shared.Core.DI
{
    public class SharedServicesInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration, ContextConfig? contextConfig = null)
        {
            // If contextConfig is null, create a new instance
            if (contextConfig == null)
            {
                contextConfig = new ContextConfig();
            }

            // Register AppSettings as a service and pass the IConfiguration instance
            services.AddSingleton<AppSettings>(new AppSettings(configuration));

            // Auth
            services.AddScoped<AuthorizationMiddleware>();
            // services.AddScoped<DevelopmentMiddleware>();
            services.AddScoped<IUserAuthenticationContext, UserAuthenticationContext>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<ITokenValidator, TokenValidator>();

            // Logging
            services.AddLogging(builder =>
            {
                builder.AddConsole(); // Logs to the console
            });

            // Message bus
            services.AddTransient<IMessageProducer, MessageProducer>();
            services.AddTransient<IMessageConsumer, MessageConsumer>();

            // Producers
            services.AddTransient<UserProducer>();
            services.AddTransient<TokenProducer>();

            // Token helper
            services.AddSingleton<TokenStatusService>();

            // User helpers
            services.AddTransient<IUserCreateService, UserCreateService>();
            services.AddTransient<IUserUpdateService, UserUpdateService>();

            // ClientDetails
            services.AddScoped<ClientDetailService>();

            if (!contextConfig.CustomTokenSessionValidator)
            {
                services.AddScoped<ITokenSessionValidator, TokenSessionValidator>();
            }

            if (contextConfig.ContainsUserAuthDetails)
            {
                services.AddTransient<IUserSecurityUpdateService, UserSecurityUpdateService>();
                services.AddTransient<IUserSecurityCreateService, UserSecurityCreateService>();
                services.AddTransient<UserSettingsProducer>();
            }
        }
    }
}