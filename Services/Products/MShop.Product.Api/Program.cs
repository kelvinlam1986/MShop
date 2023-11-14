using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using MShop.Infrastructure.EventBus;
using MShop.Infrastructure.Mongo;
using MShop.Product.Api.Handlers;
using MShop.Product.DataProvider.Repositories;
using MShop.Product.DataProvider.Services;

namespace MShop.Product.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<CreateProductHandler>();

            var rabbitOptions = new RabbitMqOptions();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitOptions);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateProductHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitOptions.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitOptions.Username);
                        hostcfg.Password(rabbitOptions.Password);
                    });

                    cfg.ReceiveEndpoint("create_product", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<CreateProductHandler>(provider);
                    });
                }));
            });


            builder.Services.AddControllers();
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