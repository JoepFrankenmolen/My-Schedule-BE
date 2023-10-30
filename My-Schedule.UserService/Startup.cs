using Microsoft.OpenApi.Models;
using My_Schedule.Shared.Core.DI;
using My_Schedule.Shared.DTO.Context;
using My_Schedule.UserService.Core.DI;

namespace My_Schedule.UserService
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
            contextConfig.ContainsUserAuthDetails = true;

            SharedServicesInstaller.Install(services, _configuration, contextConfig);
            UserServicesInstaller.Install(services, _configuration);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UserService API",
                    Version = "v1",
                    Description = "Service used for managing users."
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