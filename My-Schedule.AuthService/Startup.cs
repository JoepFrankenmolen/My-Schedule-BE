using Microsoft.OpenApi.Models;
using My_Schedule.AuthService.Core.DI;
using My_Schedule.Shared.Core.DI;
using My_Schedule.Shared.DTO.Context;

namespace My_Schedule.AuthService
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Set config settings
            var contextConfig = new ContextConfig();
            contextConfig.CustomTokenSessionValidator = true;
            contextConfig.ContainsUserAuthDetails = true;

            SharedServicesInstaller.Install(services, _configuration, contextConfig);
            AuthServicesInstaller.Install(services, _configuration);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AuthService API",
                    Version = "v1",
                    Description = "Service used for Authentication"
                });
            });

            services.AddMvc();

            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.RoutePrefix = "swagger";
                });

                //app.UseMiddleware<DevelopmentMiddleware>();

                app.UseCors("AllowVueApp");
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<Shared.Middleware.AuthorizationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}