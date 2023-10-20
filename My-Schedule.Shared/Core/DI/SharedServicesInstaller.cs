﻿using My_Schedule.Shared.Core.Interfaces;
using My_Schedule.Shared.DTO.Context;
using My_Schedule.Shared.Middleware;
using My_Schedule.Shared.Services.Authorization;
using My_Schedule.Shared.Services.Authorization.Interfaces;
using My_Schedule.Shared.Services.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using My_Schedule.Shared.Services.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

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

            /// Does not initialize:
            /// IServicesAppSettings-
            /// DBContext
            /// UserHelper
            /// TokenSesionValidator
            /// IUserAuthenticationContext

            // Register AppSettings as a service and pass the IConfiguration instance
            services.AddSingleton<AppSettings>(new AppSettings(configuration));

            services.AddScoped<AuthorizationMiddleware>();

            //services.AddScoped<DevelopmentMiddleware>();

            services.AddScoped<IUserAuthenticationContext, UserAuthenticationContext>();

            services.AddScoped<IAuthorizationService, AuthorizationService>();

            services.AddScoped<ITokenValidator, TokenValidator>();

            if(!contextConfig.CustomTokenSessionValidator)
            {
                services.AddScoped<ITokenSessionValidator, TokenSessionValidator>();
            }

            if (!contextConfig.CustomUserBasicHelper)
            {
                services.AddScoped<IUserBasicHelper, UserBasicHelper>();
            }
        }
    }
}