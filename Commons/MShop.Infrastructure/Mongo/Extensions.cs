using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MShop.Infrastructure.Mongo
{
    public static class Extensions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var configSection = configuration.GetSection("mongo");
            var mongoConfig = new MongoConfig();
            configSection.Bind(mongoConfig);

            services.AddSingleton<IMongoClient>(client =>
            {
                return new MongoClient(mongoConfig.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(database =>
            {
                var mongoClient = database.GetService<IMongoClient>();
                return mongoClient.GetDatabase(mongoConfig.Database);
            });

            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
