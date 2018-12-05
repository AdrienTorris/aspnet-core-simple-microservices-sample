namespace Actio.Common.Mongo
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public static class Extensions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();

                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddSingleton<IMongoDatabase>(c=>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });
            services.AddTransient<IDatabaseSeeder, MongoSeeder>();
            services.AddTransient<IDatabaseInitializer,MongoInitializer>();
        }
    }
}