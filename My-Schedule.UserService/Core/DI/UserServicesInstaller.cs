using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.RabbitMQ.Consumers;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Services.Users;

namespace My_Schedule.UserService.Core.DI
{
    public class UserServicesInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Use the AppSettings instance to retrieve the database connection string
            var appSettings = services.BuildServiceProvider().GetService<AppSettings>();

            // Register services
            services.AddSingleton<IDatabaseSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IAuthenticationSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IEmailSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));

            // Register the UserServiceContext and associated interfaces
            services.AddDbContext<UserServiceContext>(options =>
            {
                options.UseSqlServer(appSettings.DatabaseConnection);
            });

            // Register the interfaces with their implementations
            services.AddScoped<ITokenStatusContext>(provider => provider.GetRequiredService<UserServiceContext>());
            services.AddScoped<IUserAuthDetailContext>(provider => provider.GetRequiredService<UserServiceContext>());
            services.AddScoped<IUserContext>(provider => provider.GetRequiredService<UserServiceContext>());

            // Context builder
            services.AddTransient<IDefaultContextBuilder, DefaultContextBuilder>();

            // User services
            services.AddScoped<UserRoleService>();
            services.AddScoped<UserAdminFetchingService>();
            services.AddScoped<UserAdminService>();

            // Consumer
            services.AddSingleton<IHostedService, TokenConsumer<UserServiceContext>>();
        }
    }
}