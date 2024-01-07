using Microsoft.EntityFrameworkCore;
using My_Schedule.NotificationService.RabbitMQ;
using My_Schedule.NotificationService.Services;
using My_Schedule.NotificationService.Services.Notifications;
using My_Schedule.NotificationService.Services.Users;
using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.RabbitMQ.Consumers;

namespace My_Schedule.NotificationService.Core.DI
{
    public class NotificationServicesInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Done like this to make sure there is only one place the DatabaseConnection gets called.
            // Use the AppSettings instance to retrieve the database connection string
            var appSettings = services.BuildServiceProvider().GetService<AppSettings>();

            // probely more efficent ways of doing this but good for now
            services.AddSingleton<IDatabaseSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IAuthenticationSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IEmailSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));
            services.AddSingleton<IMessageQueueSettings, ServicesAppSettings>(sp => new ServicesAppSettings(appSettings));

            // Register the UserServiceContext and associated interfaces
            services.AddDbContext<NotificationServiceContext>(options =>
            {
                options.UseSqlServer(appSettings.DatabaseConnection);
            });

            // Register the interfaces with their implementations
            services.AddScoped<ITokenStatusContext>(provider => provider.GetRequiredService<NotificationServiceContext>());
            services.AddScoped<IUserContext>(provider => provider.GetRequiredService<NotificationServiceContext>());

            // Context builder
            services.AddTransient<IDefaultContextBuilder, DefaultContextBuilder>();

            // Notifications
            services.AddScoped<NotificationUpdateService>();
            services.AddScoped<NotificationUserPreferencesService>();

            // Senders
            services.AddTransient<NotificationDispatcher>();
            services.AddTransient<NotificationSender>();

            // Consumers
            services.AddSingleton<IHostedService, TokenConsumer<NotificationServiceContext>>();
            services.AddSingleton<IHostedService, UserConsumer<NotificationServiceContext>>();
            services.AddSingleton<IHostedService, UserSettingsConsumer<NotificationServiceContext>>();
            services.AddSingleton<IHostedService, UserActivityConsumer<NotificationServiceContext>>();

            // Custom Consumer
            services.AddSingleton<IHostedService, NotificationConsumer>();
        }
    }
}