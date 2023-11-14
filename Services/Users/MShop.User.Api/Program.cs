using GreenPipes;
using MassTransit;
using MShop.Infrastructure.EventBus;
using MShop.Infrastructure.Mongo;
using MShop.Infrastructure.Security;
using MShop.User.Api.Handlers;
using MShop.User.DataProvider.Repositories;
using MShop.User.DataProvider.Services;

namespace MShop.User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IEncrypter, Encrypter>();
            builder.Services.AddScoped<CreateUserHandler>();

            var rabbitOptions = new RabbitMqOptions();
            builder.Configuration.GetSection("rabbitmq").Bind(rabbitOptions);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateUserHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitOptions.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitOptions.Username);
                        hostcfg.Password(rabbitOptions.Password);
                    });

                    cfg.ReceiveEndpoint("add_user", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
                        ep.ConfigureConsumer<CreateUserHandler>(provider);
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

            app.UseAuthorization();


            app.MapControllers();

            var busControl = app.Services.GetService<IBusControl>();
            busControl.Start();

            var dbInitializer = app.Services.GetService<IDatabaseInitializer>();
            dbInitializer.InitializeAsync();

            app.Run();
        }
    }
}