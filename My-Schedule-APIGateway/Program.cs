using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace My_Schedule_APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add your ocelot.json configuration
            builder.Configuration.AddJsonFile("ocelot.json");

            // Add Ocelot services
            builder.Services.AddOcelot(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*           if (app.Environment.IsDevelopment())
                       {
                           app.UseSwaggerForOcelotUI();
                       }*/
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });

            app.UseHttpsRedirection();
            app.MapControllers();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}