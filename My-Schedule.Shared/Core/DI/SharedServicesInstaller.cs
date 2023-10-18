using My_Schedule.Shared.Middleware;
using My_Schedule.Shared.Services.Authorization;
using My_Schedule.Shared.Services.Authorization.Interfaces;
using My_Schedule.Shared.Services.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.Shared.Core.DI
{
    public class SharedServicesInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            /// Does not initialize:
            /// IServicesAppSettings
            /// DBContext
            /// UserHelper
            /// TokenSesionValidator
            /// IUserAuthenticationContext

            // Register AppSettings as a service and pass the IConfiguration instance
            services.AddSingleton<AppSettings>(new AppSettings(configuration));

            services.AddScoped<AuthorizationMiddleware>();

            //services.AddScoped<DevelopmentMiddleware>();

            services.AddScoped<IAuthorizationService, AuthorizationService>();

            services.AddScoped<ITokenValidator, TokenValidator>();
        }
    }
}