using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Services.Auth;
using My_Schedule.AuthService.Services.Auth.Authentication;
using My_Schedule.AuthService.Services.Auth.Tokens;
using My_Schedule.AuthService.Services.Authentication;
using My_Schedule.AuthService.Services.Confirmations;
using My_Schedule.AuthService.Services.Logs;
using My_Schedule.AuthService.Services.Notifications;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Consumers;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.Shared.Services.Users.Users;

namespace My_Schedule.AuthService.Core.DI
{
    public class AuthServicesInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Done like this to make sure there is only one place the DatabaseConnection gets called.
            // Use the AppSettings instance to retrieve the database connection string
            var appSettings = services.BuildServiceProvider().GetService<AppSettings>();

            // probely more efficent ways of doing this but good for now
            services.AddSingleton<IDatabaseSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IConfirmationSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IAuthenticationSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IUserAuthSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IEmailSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IMessageQueueSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));

            // Register the UserServiceContext and associated interfaces
            services.AddDbContext<AuthServiceContext>(options =>
            {
                options.UseSqlServer(appSettings.DatabaseConnection);
            });

            // Register the interfaces with their implementations
            services.AddScoped<IUserSecurityContext>(provider => provider.GetRequiredService<AuthServiceContext>());
            services.AddScoped<IUserContext>(provider => provider.GetRequiredService<AuthServiceContext>());
            services.AddScoped<IClientDetailsContext>(provider => provider.GetRequiredService<AuthServiceContext>());

            // Context builder
            services.AddTransient<IDefaultContextBuilder, DefaultContextBuilder>();

            // Auth
            services.AddScoped<LogoutService>();
            services.AddScoped<LoginService>();
            services.AddScoped<RegisterService>();
            services.AddScoped<HashService>();
            services.AddScoped<PasswordResetService>(); 
            services.AddScoped<LoginVerificationService>();

            // Tokens
            services.AddScoped<ITokenSessionValidator, TokenSessionService>();
            services.AddScoped<TokenSessionService>();
            services.AddScoped<TokenService>();
            services.AddScoped<TokenGenerator>();
            services.AddScoped<TokenDevelopmentGenerator>();

            // Users
            services.AddScoped<UserService>();
            services.AddScoped<IUserCreatedEvent, UserCreatedEventAuth>();

            // Confirmation
            services.AddScoped<ConfirmationService>();
            services.AddScoped<ConfirmationDispatcher>();
            services.AddScoped<EmailConfirmationService>();

            // Logs
            services.AddScoped<LoginLogService>();
            services.AddScoped<ConfirmationLogService>();

            // Notifications
            services.AddScoped<NotificationTriggerService>();
            services.AddScoped<NotificationSender>();

            // UserCreatedEvent
            services.AddTransient<IUserCreatedEvent, UserCreatedEventAuth>();

            // Consumers
            services.AddSingleton<IHostedService, UserConsumer<AuthServiceContext>>();
            services.AddSingleton<IHostedService, UserSettingsConsumer<AuthServiceContext>>();
            services.AddSingleton<IHostedService, UserActivityConsumer<AuthServiceContext>>();
        }
    }
}