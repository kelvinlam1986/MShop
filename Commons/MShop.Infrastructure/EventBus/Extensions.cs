using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MShop.Infrastructure.Command.User;
using MShop.Infrastructure.Query;

namespace MShop.Infrastructure.EventBus
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration) 
        {
            var rabbitMq = new RabbitMqOptions();
            configuration.GetSection("rabbitmq").Bind(rabbitMq);

            // establish connection with rabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMq.ConnectionString), hostcf =>
                    {
                        hostcf.Username(rabbitMq.Username);
                        hostcf.Password(rabbitMq.Password);
                    });
                }));
                x.AddRequestClient<GetProductById>();
                x.AddRequestClient<LoginUser>();
            });

            return services;
        }


    }
}
