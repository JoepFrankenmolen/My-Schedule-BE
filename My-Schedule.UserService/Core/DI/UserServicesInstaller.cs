using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Core;
using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Services.Users;
using My_Schedule.UserService.Services.Users.Helpers;

namespace My_Schedule.UserService.Core.DI
{
    public class UserServicesInstaller
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

            services.AddDbContext<ITokenStatusContext, UserServiceContext>(options =>
                options.UseSqlServer(appSettings.DatabaseConnection), ServiceLifetime.Scoped); // check of dit werkt

            services.AddScoped<IUserBasicHelper, UserHelper>();

            services.AddScoped<UserHelper>();

            services.AddScoped<UserRoleService>();

            services.AddScoped<UserFetchingService>();

            services.AddScoped<UserAdminService>();
        }
    }
}