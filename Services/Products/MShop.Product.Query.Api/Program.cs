using MassTransit;
using MShop.Infrastructure.EventBus;
using MShop.Product.Query.Api.Handlers;
using MShop.Infrastructure.Mongo;
using MShop.Product.DataProvider.Services;
using MShop.Product.DataProvider.Repositories;

namespace MShop.Product.Query.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddScoped<IProductService, ProductService>();  
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<GetProductByIdHandler>();

            // Add services to the container.
            var rabbitMq = new RabbitMqOptions();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitMq);

            // establish connection with rabbitMQ
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductByIdHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMq.ConnectionString), hostcf =>
                    {
                        hostcf.Username(rabbitMq.Username);
                        hostcf.Password(rabbitMq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
            });

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

            var dbInitializer = app.Services.GetService<IDatabaseInitializer>();
            dbInitializer.InitializeAsync();

            app.Run();
        }
    }
}