using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Services;
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
using My_Schedule.Shared.Services.Tokens.Interfaces;
using My_Schedule.Shared.Services.Users.Interfaces;

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
            services.AddSingleton<IUserSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IEmailSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));

            services.AddDbContext<AuthServiceContext>(options =>
                options.UseSqlServer(appSettings.DatabaseConnection), ServiceLifetime.Scoped);

            services.AddScoped<LogoutService>();

            services.AddScoped<LoginService>();

            services.AddScoped<RegisterService>();

            services.AddScoped<TokenService>();

            services.AddScoped<TokenGenerator>();

            services.AddScoped<UserService>();

            services.AddScoped<IUserBasicHelper, UserHelper>();

            services.AddScoped<UserHelper>(); // why

            services.AddScoped<ClientDetailService>();

            services.AddScoped<HashService>();

            services.AddScoped<LoginLogService>();

            services.AddScoped<ConfirmationLogService>();

            services.AddScoped<ITokenSessionValidator, TokenSessionService>();

            services.AddScoped<TokenSessionService>(); // why

            services.AddScoped<NotificationTriggerService>();

            services.AddScoped<TokenDevelopmentGenerator>();

            services.AddScoped<NotificationSender>();

            services.AddScoped<ConfirmationService>();

            services.AddScoped<ConfirmationDispatcher>();

            services.AddScoped<PasswordResetService>();

            services.AddScoped<LoginVerificationService>();

            services.AddScoped<EmailConfirmationService>();
        }
    }
}