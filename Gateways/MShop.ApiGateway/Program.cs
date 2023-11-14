using MassTransit;
using MShop.Infrastructure.Authentication;
using MShop.Infrastructure.EventBus;

namespace MShop.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddJwt(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddRabbitMq(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var busControl = app.Services.GetService<IBusControl>();
            busControl.Start();

            app.Run();
        }
    }
}