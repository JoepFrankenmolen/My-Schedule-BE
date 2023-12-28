using Microsoft.OpenApi.Models;
using My_Schedule.NotificationService.Core.DI;
using My_Schedule.Shared.Core.DI;
using My_Schedule.Shared.DTO.Context;

namespace My_Schedule.NotificationService
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
            contextConfig.HasClientDetailService = false;

            SharedServicesInstaller.Install(services, _configuration, contextConfig);
            NotificationServicesInstaller.Install(services, _configuration);

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

            // disable cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.RoutePrefix = "swagger";
                });

                app.UseCors("AllowAny");
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